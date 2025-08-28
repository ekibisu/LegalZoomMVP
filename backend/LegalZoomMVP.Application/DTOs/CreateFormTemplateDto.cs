using System.ComponentModel.DataAnnotations;

namespace LegalZoomMVP.Application.DTOs
{
    public class CreateFormTemplateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public bool IsPremium { get; set; }

        [Required]
        public FormSchemaDto FormSchema { get; set; } = null!;

        [Required]
        public string HtmlTemplate { get; set; } = string.Empty;
    }
}