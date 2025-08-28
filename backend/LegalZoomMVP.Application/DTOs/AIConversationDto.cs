namespace LegalZoomMVP.Application.DTOs
{
    public class AIConversationDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<AIMessageDto> Messages { get; set; } = new();
    }
}