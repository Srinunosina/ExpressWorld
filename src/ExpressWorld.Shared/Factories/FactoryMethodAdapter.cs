using ExpressWorld.Shared.FactoryMethod;
using ExpressWorld.Shared.Adapters;
using ExpressWorld.Shared.Configurations;
using Microsoft.Extensions.Options;

namespace ExpressWorld.Shared.Factories
{
    public class FactoryMethodAdapter
    {
        private readonly IEnumerable<IAdapterFactory> _adapterFactories;
        private readonly IOptionsMonitor<List<SupplierConfig>> _supplierConfigs;

        public FactoryMethodAdapter(IEnumerable<IAdapterFactory> adapterFactories, IOptionsMonitor<List<SupplierConfig>> supplierConfigs)
        {
            _adapterFactories = adapterFactories;
            _supplierConfigs = supplierConfigs;
        }

        public IEnumerable<IProductAdapter> CreateAdapters()
        {
            var adapters = new List<IProductAdapter>();
            var suppliers = _supplierConfigs.CurrentValue;

            foreach (var supplier in suppliers)
            {
                var factory = _adapterFactories.FirstOrDefault(f => f.CanHandle(supplier.DataSourceType));

                if (factory != null)
                {
                    adapters.Add(factory.CreateAdapter(supplier));
                }
                else
                {
                    throw new NotImplementedException($"Adapter for {supplier.DataSourceType} not implemented.");
                }
            }

            return adapters;
        }
    }
}
