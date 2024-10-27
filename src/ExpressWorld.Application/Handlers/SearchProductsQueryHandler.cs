using ExpressWorld.Application.DTOs;
using ExpressWorld.Application.Queries;
using ExpressWorld.Application.Services;
using MediatR;

namespace ExpressWorld.Application.Handlers
{
    public class SearchProductsQueryHandler : IRequestHandler<SearchProductsQuery, IReadOnlyCollection<ProductDTO>>
    {
        private readonly IProductService _productService;

        public SearchProductsQueryHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IReadOnlyCollection<ProductDTO>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
        {
            return await _productService.SearchProductsAsync(request, cancellationToken);
        }
        //public async Task<List<ProductDTO>> Handle(SearchProductsQuery query, CancellationToken cancellationToken)
        //{
        //    var products = await _productRepository.GetAllProductsAsync(cancellationToken);
        //    return null;
        //    //var filteredProducts = products
        //    //    //.Where(p => p.Capacity >= query.NumberOfGuests)
        //    //    .Where(p => string.IsNullOrEmpty(query.ProductName) || p.Name.Contains(query.ProductName, StringComparison.OrdinalIgnoreCase))
        //    //    .Where(p => string.IsNullOrEmpty(query.DestinationName) || p.DestinationName.Contains(query.DestinationName, StringComparison.OrdinalIgnoreCase))
        //    //    .Where(p => !query.MaxPrice.HasValue || p.Price <= query.MaxPrice);

        //    //return filteredProducts
        //    //    .Skip(query.PageIndex * query.PageSize)
        //    //    .Take(query.PageSize)
        //    //    .Select(p => new ProductDTO
        //    //    {
        //    //       // ProductName = p.Name,
        //    //       // Description = p.Description,
        //    //       // DestinationName = p.Destination,
        //    //       //// Price = p.Price,
        //    //       // Supplier = p.SupplierName
        //    //    })
        //    //    .ToList();
        //}
    }

}
