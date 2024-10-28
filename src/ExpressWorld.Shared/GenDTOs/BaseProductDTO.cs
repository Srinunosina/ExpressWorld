
namespace ExpressWorld.Shared.GenDTOs
{
    public class BaseProductDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal DiscountPrice { get; set; }  // Use as final price or calculated
        public int MaximumGuests { get; set; }
        public List<ImageDTO> Images { get; set; } // Assuming ImageDTO is already defined
    }
}
