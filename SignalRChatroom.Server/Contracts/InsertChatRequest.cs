namespace SignalRChatroom.Server.Contracts;

public record InsertChatRequest(
    string Username,
    string Message
);
