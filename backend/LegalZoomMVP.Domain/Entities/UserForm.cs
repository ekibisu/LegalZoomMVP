using System.ComponentModel.DataAnnotations;

namespace LegalZoomMVP.Domain.Entities
{
    public class UserForm
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        public int FormTemplateId { get; set; }
        
        // JSON data containing user's form responses
        [Required]
        public string FormData { get; set; } = string.Empty;
        
        public FormStatus Status { get; set; } = FormStatus.Draft;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? CompletedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual FormTemplate FormTemplate { get; set; } = null!;
    }

    public enum FormStatus
    {
        Draft = 0,
        Completed = 1,
        Exported = 2
    }
}