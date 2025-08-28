// Domain/Entities/User.cs
using System.ComponentModel.DataAnnotations;

namespace LegalZoomMVP.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        [Required]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        public string LastName { get; set; } = string.Empty;
        
        public UserRole Role { get; set; } = UserRole.Client;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual ICollection<UserForm> UserForms { get; set; } = new List<UserForm>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<AIConversation> AIConversations { get; set; } = new List<AIConversation>();
        public virtual Subscription? Subscription { get; set; }
    }

    public enum UserRole
    {
        Client = 0,
        Admin = 1,
        Lawyer = 2
    }
}