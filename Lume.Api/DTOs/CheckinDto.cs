namespace Lume.Api.DTOs
{
    public class CreateCheckinDto
    {
        public string Emotion { get; set; } = null!;
        public int EmotionalLevel { get; set; }
        public string? Notes { get; set; }
    }

    public class CheckinDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Emotion { get; set; } = null!;
        public int EmotionalLevel { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CheckinHistoryDto
    {
        public int Id { get; set; }
        public string Emotion { get; set; } = null!;
        public int EmotionalLevel { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
