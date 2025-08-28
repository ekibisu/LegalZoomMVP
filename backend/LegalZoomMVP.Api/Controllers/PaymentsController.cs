using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LegalZoomMVP.Application.DTOs;
using LegalZoomMVP.Application.Interfaces;
using LegalZoomMVP.Application.Exceptions;
using System.Security.Claims;

namespace LegalZoomMVP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("checkout-session")]
        public async Task<ActionResult<CheckoutSessionDto>> CreateCheckoutSession(CreateCheckoutSessionDto request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            
            try
            {
                var session = await _paymentService.CreateCheckoutSessionAsync(userId, request);
                return Ok(session);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPaymentHistory()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var payments = await _paymentService.GetUserPaymentsAsync(userId);

            return Ok(payments);
        }

        // TODO: Implement Stripe webhook handling
        // [HttpPost("webhook")]
        // [AllowAnonymous]
        // public async Task<IActionResult> StripeWebhook()
        // {
        //     var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        //     var stripeSignature = Request.Headers["Stripe-Signature"];
        //     // Implementation needed when Stripe service is complete
        //     return Ok();
        // }
    }
}
