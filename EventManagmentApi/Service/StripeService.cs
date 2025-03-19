using System;
using System.Threading.Tasks;
using EventManagmentApi.Models;
using EventManagmentApi.Models.DTOs;
using EventManagmentApi.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;

namespace EventManagmentApi.Service
{
    public class StripeService : IStripeService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventService _eventService;
        private readonly ILogger<StripeService> _logger;

        public StripeService(
            IConfiguration configuration,
            IEventService eventService,
            ILogger<StripeService> logger)
        {
            _configuration = configuration;
            _eventService = eventService;
            _logger = logger;

            // Configure Stripe API key from settings
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
        }

        public async Task<PaymentIntentResponseDto> CreatePaymentIntentAsync(CreatePaymentIntentRequestDto request)
        {
            try
            {
                // Get the event details to know the price
                var eventDetails = await _eventService.GetEventByIdAsync(request.EventId);
                if (eventDetails == null)
                {
                    throw new Exception($"Event with ID {request.EventId} not found");
                }

                // Calculate the total amount (Stripe requires amount in cents)
                var amount = (long)(eventDetails.Price * request.Quantity * 100);

                // Create payment intent options
                var options = new PaymentIntentCreateOptions
                {
                    Amount = amount,
                    Currency = request.Currency,
                    Description = $"Purchase of {request.Quantity} ticket(s) for {eventDetails.Name}",
                    AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                    {
                        Enabled = true,
                    },
                    Metadata = new Dictionary<string, string>
                    {
                        { "EventId", eventDetails.Id.ToString() },
                        { "Quantity", request.Quantity.ToString() }
                    }
                };

                // Create the payment intent
                var service = new PaymentIntentService();
                var paymentIntent = await service.CreateAsync(options);

                _logger.LogInformation($"Created payment intent with ID: {paymentIntent.Id} for event: {eventDetails.Name}");

                // Return the client secret and payment intent id to the client
                return new PaymentIntentResponseDto
                {
                    ClientSecret = paymentIntent.ClientSecret,
                    PaymentIntentId = paymentIntent.Id,
                    Amount = (decimal)paymentIntent.Amount / 100, // Convert back to dollars
                    Currency = paymentIntent.Currency,
                    EventName = eventDetails.Name
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating payment intent: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ConfirmPaymentAsync(PaymentConfirmationDto confirmationDto)
        {
            try
            {
                // Verify the payment intent status
                var service = new PaymentIntentService();
                var paymentIntent = await service.GetAsync(confirmationDto.PaymentIntentId);

                if (paymentIntent.Status != "succeeded")
                {
                    _logger.LogWarning($"Payment intent {confirmationDto.PaymentIntentId} has status {paymentIntent.Status}, not 'succeeded'");
                    return false;
                }

                _logger.LogInformation($"Payment confirmed: {confirmationDto.PaymentIntentId} for event: {confirmationDto.EventId}");

                // Here you would typically store the payment in your database
                // and create a ticket or registration for the user

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error confirming payment: {ex.Message}");
                return false;
            }
        }
    }
}