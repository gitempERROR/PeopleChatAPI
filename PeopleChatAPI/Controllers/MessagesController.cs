using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleChatAPI.Dto;
using PeopleChatAPI.Models.PeopleChat;
using PeopleChatAPI.Services;

namespace PeopleChatAPI.Controllers
{
    [Route("api/Messages")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly PeopleChatContext _context;

        public MessagesController(PeopleChatContext context)
        {
            _context = context;
        }

        // GET: api/Messages
        [HttpPost]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessages([FromBody] IdDto usersID)
        {
            List<Message> messages = await _context.Messages
                .Where
                (
                    message => 
                    message.SenderId == usersID.UserID1 && message.ReceaverId == usersID.UserID2
                    || message.SenderId == usersID.UserID2 && message.ReceaverId == usersID.UserID1
                )
                .ToListAsync();
            return messages.Select(message => new MessageDto(message)).ToList();
        }

        // GET: api/Messages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MessageDto>> GetMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            MessageDto messageDto = new(message);
            return messageDto;
        }

        // POST: api/Messages/Send
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Send")]
        public async Task<ActionResult<MessageDto>> PostMessage(MessageDto messageDto, [FromServices] ChatHub chatHub)
        {
            Message message = new()
            {
                SenderId = messageDto.SenderId,
                ReceaverId = messageDto.ReceaverId,
                MessageContent = messageDto.MessageContent,
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            chatHub.SendMessageToUser(messageDto.ReceaverId.ToString(), messageDto.SenderId.ToString());

            return CreatedAtAction(nameof(GetMessage), new { id = message.Id }, message);
        }

        private bool MessageExists(int id)
        {
            return _context.Messages.Any(e => e.Id == id);
        }
    }
}
