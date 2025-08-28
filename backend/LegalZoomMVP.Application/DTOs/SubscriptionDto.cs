using System.ComponentModel.DataAnnotations;

namespace LegalZoomMVP.Application.DTOs
{
    public class SubscriptionDto
    {
        public int Id { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public decimal MonthlyPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime NextBillingDate { get; set; }
    }
}