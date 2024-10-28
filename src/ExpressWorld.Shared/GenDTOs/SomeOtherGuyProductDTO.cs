
namespace ExpressWorld.Shared.GenDTOs
{
    public class SomeOtherGuyProductDTO : BaseProductDTO
    {
        public decimal Price { get; set; }
        public int Capacity { get; set; }
        public decimal DiscountPercentage { get; set; }

        // Additional properties specific to SomeOtherGuy can be added here
    }
}
