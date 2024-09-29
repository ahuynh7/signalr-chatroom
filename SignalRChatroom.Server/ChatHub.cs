using Microsoft.AspNetCore.SignalR;

namespace SignalRChatroom.Server;

public interface IChatHubMethods
{
    Task InsertChat(DateTime timestamp, string username, string message);
}

public class ChatHub : Hub<IChatHubMethods>;