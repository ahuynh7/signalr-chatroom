using Microsoft.EntityFrameworkCore;

namespace SignalRChatroom.Server.Persistence;

public interface IChatRepository
{
    Task<IList<Chat>> GetRecent();
    Task<Guid> InsertAsync(Chat chat);
    Task<bool> DeleteAll();
}

public class ChatRepository : IChatRepository
{
    private readonly SignalRChatroomDbContext _dbContext;

    public ChatRepository(SignalRChatroomDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<Chat>> GetRecent()
    {
        //fetch 15 most recent chats
        return await _dbContext.Chats
            .OrderByDescending(c => c.Timestamp)
            .Take(15)
            .ToListAsync();
    }

    public async Task<Guid> InsertAsync(Chat chat)
    {
        var id = _dbContext.Chats.Add(chat);

        await _dbContext.SaveChangesAsync();

        return id.Entity.ChatId;
    }

    public async Task<bool> DeleteAll()
    {
        _dbContext.Chats.RemoveRange(_dbContext.Chats);

        return await _dbContext.SaveChangesAsync() > 0;
    }
}
