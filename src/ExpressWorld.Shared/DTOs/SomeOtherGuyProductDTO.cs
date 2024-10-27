
namespace ExpressWorld.Shared.DTOs
{
    public class SomeOtherGuyProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductDescription { get; set; }
        public RatingStatistics RatingStatistics { get; set; }
        public decimal Price { get; set; }
        public double DiscountPercentage { get; set; }
        public int Capacity { get; set; }
        public List<string> ImageUrls { get; set; }
    }

    public class RatingStatistics
    {
        public int TotalNumberOfReviews { get; set; }
        public int TotalRating { get; set; }
    }
}
