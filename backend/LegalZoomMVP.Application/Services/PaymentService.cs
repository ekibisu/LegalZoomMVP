using LegalZoomMVP.Application.DTOs;
using LegalZoomMVP.Application.Interfaces;
using LegalZoomMVP.Application.Exceptions;
using LegalZoomMVP.Domain.Entities;

namespace LegalZoomMVP.Application.Services
{
    public class PaymentService(IPaymentRepository paymentRepository, IStripeService stripeService) : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository = paymentRepository;
        private readonly IStripeService _stripeService = stripeService;

        public async Task<CheckoutSessionDto> CreateCheckoutSessionAsync(int userId, CreateCheckoutSessionDto request)
        {
            var user = await _paymentRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("User not found");

            if (request.FormTemplateId.HasValue)
            {
                // One-time form purchase
                var template = await _paymentRepository.GetFormTemplateByIdAsync(request.FormTemplateId.Value);
                if (template == null)
                    throw new NotFoundException("Form template not found");
            }

            // Delegate to Stripe service
            return await _stripeService.CreateCheckoutSessionAsync(userId, request, "https://localhost:3000"); // This should come from configuration
        }

        public async Task<PaymentDto> ProcessPaymentSuccessAsync(string sessionId)
        {
            return await _stripeService.ProcessPaymentSuccessAsync(sessionId);
        }

        public async Task<SubscriptionDto> ProcessSubscriptionSuccessAsync(string sessionId)
        {
            return await _stripeService.ProcessSubscriptionSuccessAsync(sessionId);
        }

        public async Task<PaymentDto> GetPaymentAsync(int paymentId)
        {
            // This would need to be implemented in the repository
            throw new NotImplementedException("GetPaymentAsync not yet implemented");
        }

        public async Task<IEnumerable<PaymentDto>> GetUserPaymentsAsync(int userId)
        {
            var payments = await _paymentRepository.GetPaymentsByUserIdAsync(userId);
            return payments.Select(p => new PaymentDto
            {
                Id = p.Id,
                Amount = p.Amount,
                Currency = p.Currency,
                Status = p.Status.ToString(),
                Type = p.Type.ToString(),
                FormTemplateName = p.FormTemplate?.Name,
                SubscriptionPlan = p.Subscription?.PlanName,
                CreatedAt = p.CreatedAt,
                CompletedAt = p.CompletedAt
            });
        }

        public async Task<SubscriptionDto> GetUserSubscriptionAsync(int userId)
        {
            // This would need to be implemented in the repository
            throw new NotImplementedException("GetUserSubscriptionAsync not yet implemented");
        }
    }
}