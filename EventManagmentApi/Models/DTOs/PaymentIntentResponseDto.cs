namespace EventManagmentApi.Models.DTOs
{
    public class PaymentIntentResponseDto
    {
        public string ClientSecret { get; set; }
        public string PaymentIntentId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string EventName { get; set; }
    }
}
