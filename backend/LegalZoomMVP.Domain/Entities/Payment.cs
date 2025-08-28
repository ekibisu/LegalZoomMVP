using System.ComponentModel.DataAnnotations;

namespace LegalZoomMVP.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        [Required]
        public string StripePaymentIntentId { get; set; } = string.Empty;
        
        public decimal Amount { get; set; }
        
        [Required]
        public string Currency { get; set; } = "USD";
        
        public PaymentStatus Status { get; set; }
        
        public PaymentType Type { get; set; }
        
        // For form purchases
        public int? FormTemplateId { get; set; }
        
        // For subscription payments
        public int? SubscriptionId { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? CompletedAt { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual FormTemplate? FormTemplate { get; set; }
        public virtual Subscription? Subscription { get; set; }
    }

    public enum PaymentStatus
    {
        Pending = 0,
        Completed = 1,
        Failed = 2,
        Refunded = 3
    }

    public enum PaymentType
    {
        OneTime = 0,
        Subscription = 1
    }
}