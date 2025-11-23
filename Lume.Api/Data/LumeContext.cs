using Lume.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Lume.Api.Data
{
    public class LumeContext : DbContext
    {
        public LumeContext(DbContextOptions<LumeContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Checkin> Checkins { get; set; } = null!;
        public DbSet<ChatMessage> ChatMessages { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User entity
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);
            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(255);

            // Checkin entity
            modelBuilder.Entity<Checkin>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<Checkin>()
                .HasOne(c => c.User)
                .WithMany(u => u.Checkins)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Checkin>()
                .Property(c => c.Emotion)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<Checkin>()
                .Property(c => c.EmotionalLevel)
                .IsRequired();

            // ChatMessage entity
            modelBuilder.Entity<ChatMessage>()
                .HasKey(cm => cm.Id);
            modelBuilder.Entity<ChatMessage>()
                .HasOne(cm => cm.User)
                .WithMany(u => u.ChatMessages)
                .HasForeignKey(cm => cm.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ChatMessage>()
                .Property(cm => cm.SenderType)
                .IsRequired()
                .HasMaxLength(20);
            modelBuilder.Entity<ChatMessage>()
                .Property(cm => cm.Message)
                .IsRequired();
        }
    }
}
