using PeopleChatAPI.Models;

namespace PeopleChatAPI.Dto
{
    public record MessageDto
    {
        public int Id { get; set; }

        public int SenderId { get; set; }

        public int ReceaverId { get; set; }

        public string MessageContent { get; set; } = null!;

        public MessageDto() { }

        public MessageDto(Message message)
        {
            Id = message.Id;
            SenderId = message.SenderId;
            ReceaverId = message.ReceaverId;
            MessageContent = message.MessageContent;
        }
    }
}
