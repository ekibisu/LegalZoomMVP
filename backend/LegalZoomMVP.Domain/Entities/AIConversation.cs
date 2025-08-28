using System.ComponentModel.DataAnnotations;

namespace LegalZoomMVP.Domain.Entities
{
    public class AIConversation
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        [Required]
        public string Title { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual ICollection<AIMessage> Messages { get; set; } = new List<AIMessage>();
    }
}