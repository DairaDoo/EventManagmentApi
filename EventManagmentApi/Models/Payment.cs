namespace EventManagmentApi.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string PaymentIntentId { get; set; }
        public string CustomerEmail { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
