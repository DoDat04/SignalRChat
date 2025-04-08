using Microsoft.EntityFrameworkCore;
using SignalRChat.Models.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<ConversationMember> ConversationMembers { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<OnlineUser> OnlineUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Thiết lập khóa chính cho ConversationMembers (PK: conversationId, userId)
        modelBuilder.Entity<ConversationMember>()
            .HasKey(cm => new { cm.ConversationId, cm.UserId });

        // Thiết lập quan hệ
        modelBuilder.Entity<ConversationMember>()
            .HasOne(cm => cm.Conversation)
            .WithMany(c => c.Members)
            .HasForeignKey(cm => cm.ConversationId);

        modelBuilder.Entity<ConversationMember>()
            .HasOne(cm => cm.User)
            .WithMany(u => u.ConversationMembers)
            .HasForeignKey(cm => cm.UserId);
    }
}
