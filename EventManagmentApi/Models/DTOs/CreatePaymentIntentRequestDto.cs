namespace EventManagmentApi.Models.DTOs
{
    public class CreatePaymentIntentRequestDto
    {
        public int EventId { get; set; }
        public string Currency { get; set; } = "usd";
        public int Quantity { get; set; } = 1;
    }
}
