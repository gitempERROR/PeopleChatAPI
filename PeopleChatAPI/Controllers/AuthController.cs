using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using PeopleChatAPI.Models;

namespace PeopleChatAPI.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly PeopleChatContext _context;

        public AuthController(PeopleChatContext context)
        {
            _context = context;
        }

        // GET: api/Auth
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Auth>>> GetAuths()
        {
            return await _context.Auths.ToListAsync();
        }

        // GET: api/Auth/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Auth>> GetAuth(int id)
        {
            var auth = await _context.Auths.FindAsync(id);

            if (auth == null)
            {
                return NotFound();
            }

            return auth;
        }

        // POST: api/Auth
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostAuth(Auth auth)
        {
            Auth? newAuth = new Auth();
            User? user = new User();
            try
            {
                newAuth = await _context.Auths.Where(x => x.UserLogin == auth.UserLogin && x.UserPassword == auth.UserPassword).FirstOrDefaultAsync();
                user = await _context.Users.Where(x => x.AuthId == newAuth!.Id).FirstOrDefaultAsync();

                if (user != null)
                {
                    return user;
                }

                return NotFound();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
