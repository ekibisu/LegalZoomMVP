using System.ComponentModel.DataAnnotations;

namespace LegalZoomMVP.Application.DTOs
{
    public class CheckoutSessionDto
    {
        public string SessionId { get; set; } = string.Empty;
        public string PublishableKey { get; set; } = string.Empty;
        public string SessionUrl { get; set; } = string.Empty;
    }
}