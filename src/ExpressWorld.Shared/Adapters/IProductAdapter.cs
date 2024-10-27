using ExpressWorld.Core.Entities;

namespace ExpressWorld.Shared.Adapters
{
    public interface IProductAdapter
    {
        Task<IEnumerable<Product>> LoadProductsAsync(CancellationToken cancellationToken);
    }
}
