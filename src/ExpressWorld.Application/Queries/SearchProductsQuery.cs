using ExpressWorld.Application.DTOs;
using MediatR;

namespace ExpressWorld.Application.Queries
{
    public class SearchProductsQuery : IRequest<IReadOnlyCollection<ProductDTO>>
    {
        public int NumberOfGuests { get; set; }
        public string ProductName { get; set; }
        public string DestinationName { get; set; }
        public string Supplier { get; set; }
        public decimal? MaxPrice { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 0;
    }
}
