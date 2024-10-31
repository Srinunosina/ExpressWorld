using ExpressWorld.Shared.Adapters;
using ExpressWorld.Shared.Configurations;

namespace ExpressWorld.Shared.FactoryMethod
{
    public interface IAdapterFactory
    {
        bool CanHandle(string dataSourceType);
        IProductAdapter CreateAdapter(SupplierConfig supplier);
    }
}
