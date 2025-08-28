using System.ComponentModel.DataAnnotations;

namespace LegalZoomMVP.Application.DTOs
{
    public class CreateAIConversationDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string InitialMessage { get; set; } = string.Empty;
    }
}