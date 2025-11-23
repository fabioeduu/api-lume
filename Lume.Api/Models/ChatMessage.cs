namespace Lume.Api.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SenderType { get; set; } = null!; // "user" ou "system"
        public string Message { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign key
        public User? User { get; set; }
    }
}
