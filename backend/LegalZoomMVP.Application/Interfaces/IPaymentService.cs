using LegalZoomMVP.Application.DTOs;

namespace LegalZoomMVP.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<CheckoutSessionDto> CreateCheckoutSessionAsync(int userId, CreateCheckoutSessionDto request);
        Task<PaymentDto> ProcessPaymentSuccessAsync(string sessionId);
        Task<SubscriptionDto> ProcessSubscriptionSuccessAsync(string sessionId);
        Task<PaymentDto> GetPaymentAsync(int paymentId);
        Task<IEnumerable<PaymentDto>> GetUserPaymentsAsync(int userId);
        Task<SubscriptionDto> GetUserSubscriptionAsync(int userId);
    }
}