using ExpressWorld.Core.Entities;

namespace ExpressWorld.Application.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken);
    }
}
