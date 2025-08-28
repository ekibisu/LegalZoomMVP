namespace LegalZoomMVP.Application.DTOs
{
    public class AIMessageDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}