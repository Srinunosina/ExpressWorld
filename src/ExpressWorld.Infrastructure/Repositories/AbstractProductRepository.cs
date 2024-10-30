using ExpressWorld.Application.Repositories;
using ExpressWorld.Core.Entities;
using ExpressWorld.Shared.AbstractFactory;
using ExpressWorld.Shared.Adapters;
using ExpressWorld.Shared.Configurations;
using Microsoft.Extensions.Options;

namespace ExpressWorld.Infrastructure.Repositories
{
    public class AbstractProductRepository : IAbstractProductRepository
    {
        private readonly IEnumerable<IAdapterFactory> _adapterFactories;
        private readonly IEnumerable<IProductAdapter> _adapters;

        public AbstractProductRepository(IEnumerable<IAdapterFactory> adapterFactories, IOptionsMonitor<List<SupplierConfig>> supplierConfigs)
        {
            _adapterFactories = adapterFactories;

            // Use each factory to create adapters based on the supplier configuration
            var suppliers = supplierConfigs.CurrentValue;
            _adapters = suppliers.SelectMany(supplierConfig =>
                _adapterFactories
                    .Where(factory => factory.CanHandle(supplierConfig.DataSourceType))
                    .Select(factory => factory.CreateAdapter(supplierConfig))
            ).ToList();
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken)
        {
            var tasks = new List<Task<IEnumerable<Product>>>();
            using var semaphore = new SemaphoreSlim(5); // Limit to 5 concurrent tasks

            foreach (var adapter in _adapters)
            {
                await semaphore.WaitAsync(cancellationToken); // Wait for an available slot
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
