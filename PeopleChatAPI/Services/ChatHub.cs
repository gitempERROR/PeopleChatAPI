using Microsoft.AspNetCore.SignalR;
using System.Security.Policy;

namespace PeopleChatAPI.Services
{
    public class ChatHub : Hub
    {
        private Dictionary<String, String> _usersDict = [];

        public override async Task OnConnectedAsync()
        {
            string userId = Context.GetHttpContext()?.Request.Headers["userId"].ToString();
            string connectionId = Context.ConnectionId;
            _usersDict.Add(connectionId, userId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Удаляем запись из словаря при отключении
            _usersDict.Remove(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessageToUser(string userId, string senderId)
        {
            foreach (var connection in _usersDict)
            {
                if (connection.Value == userId)
                {
                    await Clients.Client(connection.Key).SendAsync("ReceiveMessage", senderId);
                }
            }
        }
    }
}
