using System.ComponentModel.DataAnnotations;

namespace LegalZoomMVP.Application.DTOs
{
    public class FormSchemaDto
    {
        public List<FormFieldDto> Fields { get; set; } = new();
    }
}