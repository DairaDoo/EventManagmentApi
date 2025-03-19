using EventManagmentApi.Models.DTOs;
using System.Threading.Tasks;

namespace EventManagmentApi.Service.Interfaces
{
    public interface IStripeService
    {
        Task<PaymentIntentResponseDto> CreatePaymentIntentAsync(CreatePaymentIntentRequestDto request);
        Task<bool> ConfirmPaymentAsync(PaymentConfirmationDto confirmationDto);
    }
}
