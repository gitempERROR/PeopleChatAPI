using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // PUT: api/Auth/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuth(int id, Auth auth)
        {
            if (id != auth.Id)
            {
                return BadRequest();
            }

            _context.Entry(auth).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Auth
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Auth>> PostAuth(Auth auth)
        {
            _context.Auths.Add(auth);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAuth), new { id = auth.Id }, auth);
        }

        // DELETE: api/Auths/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuth(int id)
        {
            var auth = await _context.Auths.FindAsync(id);
            if (auth == null)
            {
                return NotFound();
            }

            _context.Auths.Remove(auth);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthExists(int id)
        {
            return _context.Auths.Any(e => e.Id == id);
        }
    }
}
