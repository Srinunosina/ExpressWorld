
//using ExpressWorld.Shared.DTOs;

using ExpressWorld.Shared.DTOs;

namespace ExpressWorld.Shared.GenDTOs
{
    public class BigGuyProductDTO : BaseProductDTO
    {
        public PriceDetails Price { get; set; } // Assuming PriceDetails is a class containing pricing details
        public ProductDetailData ProductDetailData { get; set; } // Assuming it contains specific details

        // Additional properties specific to BigGuy can be added here
    }
}
