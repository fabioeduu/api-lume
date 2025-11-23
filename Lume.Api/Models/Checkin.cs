namespace Lume.Api.Models
{
    public class Checkin
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Emotion { get; set; } = null!; // alegre, triste, ansioso, calmo, frustrado, esperançoso
        public int EmotionalLevel { get; set; } // 1-10 scale
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Foreign key
        public User? User { get; set; }
    }
}
