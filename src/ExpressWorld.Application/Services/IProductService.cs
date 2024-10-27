using ExpressWorld.Application.DTOs;
using ExpressWorld.Application.Queries;

namespace ExpressWorld.Application.Services
{
    public interface IProductService
    {
        Task<IReadOnlyCollection<ProductDTO>> SearchProductsAsync(SearchProductsQuery query, CancellationToken cancellationToken);
    }
}
