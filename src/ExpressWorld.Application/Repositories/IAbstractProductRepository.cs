using ExpressWorld.Core.Entities;

namespace ExpressWorld.Application.Repositories
{
    public interface IAbstractProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken);
    }
}
