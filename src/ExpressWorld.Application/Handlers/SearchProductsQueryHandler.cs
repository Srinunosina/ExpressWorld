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
    }
}
