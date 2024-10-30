using AutoMapper;
using ExpressWorld.Application.DTOs;
using ExpressWorld.Application.Queries;
using ExpressWorld.Application.Repositories;

namespace ExpressWorld.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IAbstractProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IAbstractProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// This method retrieves all products from the repository and then applies various filters and returns the results are paginated 
        /// using PageIndex and PageSize to return only the relevant subset.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<ProductDTO>> SearchProductsAsync(SearchProductsQuery query, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllProductsAsync(cancellationToken);

            if (query.NumberOfGuests > 0)
            {
                products = products.Where(p => p.NumberOfGuests >= query.NumberOfGuests); // Assuming you have MaxGuests in ProductDTO
            }

            if (!string.IsNullOrEmpty(query.ProductName))
            {
                products = products.Where(p => p.Name.Contains(query.ProductName, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(query.DestinationName))
            {
                products = products.Where(p => p.DestinationName.Contains(query.DestinationName, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(query.Supplier))
            {
                products = products.Where(p => p.SupplierName.Equals(query.Supplier, StringComparison.OrdinalIgnoreCase));
            }

            if (query.MaxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= query.MaxPrice.Value);
            }

            var paginatedProducts = products
                .Skip(query.PageIndex * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            return _mapper.Map<IReadOnlyCollection<ProductDTO>>(paginatedProducts);
        }
    }
}
