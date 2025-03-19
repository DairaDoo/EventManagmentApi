namespace EventManagmentApi.Models.DTOs
{
    public class PaymentConfirmationDto
    {
        public string PaymentIntentId { get; set; }
        public int EventId { get; set; }
        public string CustomerEmail { get; set; }
        public int Quantity { get; set; }
    }
}
