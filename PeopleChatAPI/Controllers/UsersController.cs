using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleChatAPI.Dto;
using PeopleChatAPI.Models;

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
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            try
            {
                List<User> users = await _context.Users.ToListAsync();
                List<UserDto> userDtos = users.Select(user => new UserDto(user)).ToList();
                return userDtos;
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            try
            {
                User? user = await _context.Users.FirstOrDefaultAsync(item => item.Id == id);
                if (user == null) return NotFound();
                return user;
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
