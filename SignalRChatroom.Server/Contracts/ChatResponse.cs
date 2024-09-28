namespace SignalRChatroom.Server.Contracts;

public record ChatResponse(
    DateTime Timestamp,
    string Username,
    string Message
);

