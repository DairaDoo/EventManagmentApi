namespace EventManagmentApi.Models.DTOs
{
    public class EventCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public IFormFile Image { get; set; }

    }
}
