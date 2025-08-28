using System.ComponentModel.DataAnnotations;

namespace LegalZoomMVP.Application.DTOs
{
    public class UserFormDto
    {
        public int Id { get; set; }
        public string FormTemplateName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public Dictionary<string, object> FormData { get; set; } = new();
    }
}