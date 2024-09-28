using Microsoft.EntityFrameworkCore;

namespace SignalRChatroom.Server.Persistence;

public interface IChatRepository
{
    Task<IList<Chat>> GetAll();
    Task<Guid> InsertAsync(Chat chat);
}

public class ChatRepository : IChatRepository
{
    private readonly SignalRChatroomDbContext _dbContext;

    public ChatRepository(SignalRChatroomDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<Chat>> GetAll()
    {
        //fetch 15 most recent chats
        return await _dbContext.Chats
            .OrderBy(c => c.Timestamp)
            .Take(15)
            .ToListAsync();
    }

    public async Task<Guid> InsertAsync(Chat chat)
    {
        var id = _dbContext.Chats.Add(chat);

        await _dbContext.SaveChangesAsync();

        return id.Entity.ChatId;
    }
}
