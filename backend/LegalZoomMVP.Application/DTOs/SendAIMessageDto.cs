using System.ComponentModel.DataAnnotations;

namespace LegalZoomMVP.Application.DTOs
{
    public class SendAIMessageDto
    {
        [Required]
        public string Message { get; set; } = string.Empty;
    }
}