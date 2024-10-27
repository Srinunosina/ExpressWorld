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
            var tasks = new List<Task<IEnumerable<Product>>>();
            using var semaphore = new SemaphoreSlim(5); // Limit to 5 concurrent tasks

            foreach (var adapter in _adapters)
            {
                await semaphore.WaitAsync(cancellationToken); // Wait for available slot
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        return await adapter.LoadProductsAsync(cancellationToken);
                    }
                    finally
                    {
                        semaphore.Release(); // Release slot
                    }
                }));
            }

            var productLists = await Task.WhenAll(tasks);
            return productLists.SelectMany(products => products);
        }
    }
}
