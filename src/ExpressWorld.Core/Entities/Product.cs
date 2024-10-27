namespace ExpressWorld.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DestinationName { get; set; } 
        public decimal Price { get; set; }
        public int NumberOfGuests { get; set; }
        public string SupplierName { get; set; }
        public List<string> ImageUrls { get; set; }
    }
}
