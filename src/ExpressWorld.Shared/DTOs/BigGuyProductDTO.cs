
namespace ExpressWorld.Shared.DTOs
{
    public class BigGuyProductDTO
    {
        public ProductDetailData ProductDetailData { get; set; }
        public PriceData Price { get; set; }
        public List<string> Photos { get; set; }
    }

    public class ProductDetailData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductDescription { get; set; }
        public double AverageStars { get; set; }
        public int Capacity { get; set; }
    }

    public class PriceData
    {
        public decimal Amount { get; set; }
        public double AppliedDiscount { get; set; }
    }
}
