using ExpressWorld.Application.DTOs;
using ExpressWorld.Application.Repositories;
using ExpressWorld.Core.Entities;
using ExpressWorld.Shared.Adapters;
using ExpressWorld.Shared.Factories;

namespace ExpressWorld.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IEnumerable<IProductAdapter> _adapters;
        public ProductRepository(AdapterFactory adapterFactory)
        {
            // Use the AdapterFactory to create the adapters
            _adapters = adapterFactory.CreateAdapters().ToList();
        }
        public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken)
        {
            var products = new List<Product>();
            foreach (var adapter in _adapters)
            {
                products.AddRange(await adapter.LoadProductsAsync(cancellationToken));
            }

            return products;
        }
    }
}
