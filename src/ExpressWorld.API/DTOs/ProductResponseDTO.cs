namespace ExpressWorld.API.DTOs
{
    public class ProductResponseDTO
    {
        //        •	Name of the product
        //•	Description of the product
        //•	Name of the destination where the product is located
        //•	Price of the product in the specified currency
        //•	Supplier name
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string ProductDestination { get; set; }
        public Decimal? Price { get; set; }
        public string SupplierName { get; set; }

    }
}
