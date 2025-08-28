using System.ComponentModel.DataAnnotations;

namespace LegalZoomMVP.Domain.Entities
{
    public class FormTemplate
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public string Category { get; set; } = string.Empty;
        
        public decimal Price { get; set; }
        
        public bool IsPremium { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        // JSON structure for form fields
        [Required]
        public string FormSchema { get; set; } = string.Empty;
        
        // HTML template for PDF generation
        public string HtmlTemplate { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        public int CreatedByUserId { get; set; }
        
        // Navigation properties
        public virtual User CreatedBy { get; set; } = null!;
        public virtual ICollection<UserForm> UserForms { get; set; } = new List<UserForm>();
    }
}