using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            List<User> users = await _context.Users.ToListAsync();
            List<UserDto> userDtos = users.Select(user => new UserDto(user)).ToList();
            return userDtos;
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(item => item.Id == id);
            if (user == null) return NotFound();
            return user;
        }
    }
}
