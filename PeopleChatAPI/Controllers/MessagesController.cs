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
            foreach (Message message in messages)
            {
                if (message.ReceaverId == usersID.UserID1)
                    message.IsRead = true;
            }
            await _context.SaveChangesAsync();
            return messages.Select(message => new MessageDto(message)).ToList();
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
                Message1 = messageDto.MessageContent,
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            await chatHub.SendMessageToUser(messageDto.ReceaverId.ToString(), messageDto.SenderId.ToString());

            MessageDto newMessageDto = new(message);
            return newMessageDto;
        }
    }
}
