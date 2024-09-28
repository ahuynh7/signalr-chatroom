using Microsoft.EntityFrameworkCore;

namespace SignalRChatroom.Server.Persistence;
    
public class SignalRChatroomDbContext : DbContext
{
    public SignalRChatroomDbContext(DbContextOptions<SignalRChatroomDbContext> options)
        : base(options) { } 

    public DbSet<Chat> Chats { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureChat();
    }
}
