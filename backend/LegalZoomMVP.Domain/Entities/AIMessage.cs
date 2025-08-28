using System.ComponentModel.DataAnnotations;

namespace LegalZoomMVP.Domain.Entities
{
    public class AIMessage
    {
        public int Id { get; set; }
        
        public int ConversationId { get; set; }
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        public MessageRole Role { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual AIConversation Conversation { get; set; } = null!;
    }

    public enum MessageRole
    {
        User = 0,
        Assistant = 1,
        System = 2
    }
}