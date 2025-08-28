using System.ComponentModel.DataAnnotations;

namespace LegalZoomMVP.Application.DTOs
{
    public class UpdateFormTemplateDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public decimal? Price { get; set; }
        public bool? IsPremium { get; set; }
        public FormSchemaDto? FormSchema { get; set; }
        public string? HtmlTemplate { get; set; }
        public bool? IsActive { get; set; }
    }
}