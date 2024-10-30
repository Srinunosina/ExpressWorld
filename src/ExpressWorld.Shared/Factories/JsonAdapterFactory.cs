using AutoMapper;
using ExpressWorld.Shared.AbstractFactory;
using ExpressWorld.Shared.Adapters;
using ExpressWorld.Shared.Configurations;

namespace ExpressWorld.Shared.Factories
{
    public class JsonAdapterFactory : IAdapterFactory
    {
        private readonly IMapper _mapper;

        public JsonAdapterFactory(IMapper mapper)
        {
            _mapper = mapper;
        }

        public bool CanHandle(string dataSourceType) => dataSourceType.Equals("JSON", StringComparison.OrdinalIgnoreCase);

        public IProductAdapter CreateAdapter(SupplierConfig supplier)
        {
            if (supplier.DtoTypeResolved == null)
            {
                throw new InvalidOperationException($"Could not resolve DTO type '{supplier.DtoType}'.");
            }
            var adapterType = typeof(JSONSupplierAdapter<>).MakeGenericType(supplier.DtoTypeResolved);
            return (IProductAdapter)Activator.CreateInstance(adapterType, supplier, _mapper);
        }
    }
}
