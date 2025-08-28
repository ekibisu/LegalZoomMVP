using LegalZoomMVP.Application.DTOs;

namespace LegalZoomMVP.Application.Interfaces
{
    public interface IStripeService
    {
        Task<CheckoutSessionDto> CreateCheckoutSessionAsync(int userId, CreateCheckoutSessionDto request, string frontendBaseUrl);
        Task<PaymentDto> ProcessPaymentSuccessAsync(string sessionId);
        Task<SubscriptionDto> ProcessSubscriptionSuccessAsync(string sessionId);
        Task<string> GetStripePriceId(string subscriptionPlan);
    }
} 