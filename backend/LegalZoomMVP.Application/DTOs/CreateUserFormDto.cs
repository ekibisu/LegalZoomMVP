using System.ComponentModel.DataAnnotations;

namespace LegalZoomMVP.Application.DTOs
{
    public class CreateUserFormDto
    {
        [Required]
        public int FormTemplateId { get; set; }

        [Required]
        public Dictionary<string, object> FormData { get; set; } = new();
    }
}