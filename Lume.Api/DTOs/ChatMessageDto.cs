namespace Lume.Api.DTOs
{
    public class CreateChatMessageDto
    {
        public string Message { get; set; } = null!;
    }

    public class ChatMessageDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SenderType { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
