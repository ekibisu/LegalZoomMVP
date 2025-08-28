using System.ComponentModel.DataAnnotations;

namespace LegalZoomMVP.Application.DTOs
{
    public class CreateCheckoutSessionDto
    {
        public int? FormTemplateId { get; set; }
        public string? SubscriptionPlan { get; set; }
    }
}