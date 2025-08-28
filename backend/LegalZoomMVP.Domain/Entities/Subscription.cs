using System.ComponentModel.DataAnnotations;

namespace LegalZoomMVP.Domain.Entities
{
    public class Subscription
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        [Required]
        public string StripeSubscriptionId { get; set; } = string.Empty;
        
        [Required]
        public string PlanName { get; set; } = string.Empty;
        
        public decimal MonthlyPrice { get; set; }
        
        public SubscriptionStatus Status { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public DateTime NextBillingDate { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }

    public enum SubscriptionStatus
    {
        Active = 0,
        Cancelled = 1,
        Expired = 2,
        PastDue = 3
    }
}