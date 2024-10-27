
namespace ExpressWorld.Shared.DTOs
{
    public class TourGuyProductDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double AverageRating { get; set; }
        public decimal RegularPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public int MaximumGuests { get; set; }
        public List<ImageDTO> Images { get; set; }
    }

    public class ImageDTO
    {
        public string Url { get; set; }
        public int DisplayOrder { get; set; }
    }
}
