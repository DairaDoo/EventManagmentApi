using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventManagmentApi.Models.DTOs;
using EventManagmentApi.Service.Interfaces;
using Microsoft.Extensions.Logging;

namespace EventManagmentApi.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IStripeService _stripeService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(
            IStripeService stripeService,
            ILogger<PaymentController> logger)
        {
            _stripeService = stripeService;
            _logger = logger;
        }

        [HttpPost("create-payment-intent")]
        public async Task<ActionResult<PaymentIntentResponseDto>> CreatePaymentIntent(CreatePaymentIntentRequestDto request)
        {
            try
            {
                var response = await _stripeService.CreatePaymentIntentAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating payment intent: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your payment.");
            }
        }

        [HttpPost("confirm-payment")]
        public async Task<ActionResult> ConfirmPayment(PaymentConfirmationDto confirmationDto)
        {
            try
            {
                var success = await _stripeService.ConfirmPaymentAsync(confirmationDto);

                if (success)
                {
                    return Ok(new { Success = true, Message = "Payment confirmed successfully" });
                }
                else
                {
                    return BadRequest(new { Success = false, Message = "Payment could not be confirmed" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error confirming payment: {ex.Message}");
                return StatusCode(500, "An error occurred while confirming your payment.");
            }
        }
    }
}