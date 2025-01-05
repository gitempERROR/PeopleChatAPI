using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleChatAPI.Dto;
using PeopleChatAPI.Models.PeopleChat;

namespace PeopleChatAPI.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly PeopleChatContext _context;

        public UsersController(PeopleChatContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<UserDto>>> GetUsers([FromHeader] int userId)
        {
            try
            {
                List<User> users = await _context.Users.ToListAsync();
                List<UserDto> userDtos = users.Select(user => new UserDto(user)).ToList();
                foreach (UserDto userDto in userDtos)
                {
                    int count = await _context.Messages.Where(message =>
                            message.SenderId == userDto.Id && message.ReceaverId == userId && message.IsRead == false
                        ).CountAsync();
                    userDto.NotReadMessages = count;
                }
                return userDtos;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Update")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(UserDto userData)
        {
            try
            {
                User? user = await _context.Users.FirstOrDefaultAsync(item => item.Id == userData.Id);
                if (user == null) return NotFound();

                user.BirthDate = userData.BirthDate;
                user.UserLastname = userData.UserLastname;
                user.UserFirstname = userData.UserFirstname;
                user.Image = userData.Image;
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
