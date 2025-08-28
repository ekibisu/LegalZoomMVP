using System.ComponentModel.DataAnnotations;

namespace LegalZoomMVP.Application.DTOs
{
    public class UpdateUserFormDto
    {
        [Required]
        public Dictionary<string, object> FormData { get; set; } = new();

        public bool IsCompleted { get; set; }
    }
}