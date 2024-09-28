using Microsoft.AspNetCore.SignalR;

namespace SignalRChatroom.Server;

public interface IChatHubMethods
{
    Task InsertChat(string username, string message);
}

public class ChatHub : Hub<IChatHubMethods>;
