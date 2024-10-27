using ExpressWorld.Core.Entities;
using ExpressWorld.Infrastructure.Configurations;
using ExpressWorld.Infrastructure.Factories;
using Microsoft.Extensions.Options;

namespace ExpressWorld.Infrastructure.DataSources
{
    public class DataSourceManager
    {
        private readonly SupplierFactory _factory;
        private readonly List<SupplierConfig> _supplierConfigs;

        public DataSourceManager(IOptionsMonitor<List<SupplierConfig>> supplierConfigOptions, SupplierFactory factory)
        {
            _supplierConfigs = supplierConfigOptions.CurrentValue;
            _factory = factory;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            var tasks = _supplierConfigs.Select(config =>
            {
                var adapter = _factory.CreateAdapter(config);
                return adapter.FetchProductsAsync();
            });

            var results = await Task.WhenAll(tasks);
            return results.SelectMany(r => r);
        }
    }


}
