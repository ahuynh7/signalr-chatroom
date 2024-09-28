using Microsoft.EntityFrameworkCore;

namespace SignalRChatroom.Server.Persistence;

public class Chat
{
    public Guid ChatId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Username { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public static class ChatConfiguration
{
    public static void ConfigureChat(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.ChatId);
            entity.Property(e => e.ChatId).ValueGeneratedOnAdd();

            entity.Property(e => e.Timestamp)
                .IsRequired();

            entity.Property(e => e.Message)
                .IsRequired()
                .HasMaxLength(21);

            entity.Property(e => e.Message)
                .IsRequired()
                .HasMaxLength(250);
        });
    }
}
