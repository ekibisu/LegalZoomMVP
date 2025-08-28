using LegalZoomMVP.Application.DTOs;
using LegalZoomMVP.Application.Interfaces;

namespace LegalZoomMVP.Infrastructure.Services
{
    public class StripeService : IStripeService
    {
        public Task<CheckoutSessionDto> CreateCheckoutSessionAsync(int userId, CreateCheckoutSessionDto request, string frontendBaseUrl)
        {
            // Stub implementation: return default CheckoutSessionDto
            return Task.FromResult(new CheckoutSessionDto());
        }

        public Task<PaymentDto> ProcessPaymentSuccessAsync(string sessionId)
        {
            // Stub implementation: return default PaymentDto
            return Task.FromResult(new PaymentDto());
        }

        public Task<SubscriptionDto> ProcessSubscriptionSuccessAsync(string sessionId)
        {
            // Stub implementation: return default SubscriptionDto
            return Task.FromResult(new SubscriptionDto());
        }

        public Task<string> GetStripePriceId(string subscriptionPlan)
        {
            // Stub implementation: return empty string
            return Task.FromResult(string.Empty);
        }
    }
}
