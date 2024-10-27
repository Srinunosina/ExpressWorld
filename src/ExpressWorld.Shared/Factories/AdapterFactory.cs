using AutoMapper;
using ExpressWorld.Shared.Adapters;
using ExpressWorld.Shared.Configurations;
using Microsoft.Extensions.Configuration;

namespace ExpressWorld.Shared.Factories
{
    public class AdapterFactory
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AdapterFactory(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public IEnumerable<IProductAdapter> CreateAdapters()
        {
            var adapters = new List<IProductAdapter>();
            var suppliers = _configuration.GetSection("Suppliers").Get<IEnumerable<SupplierConfig>>();

            foreach (var supplier in suppliers)
            {
                IProductAdapter adapter = supplier.DataSourceType.ToUpper() switch
                {
                    "JSON" => CreateJsonAdapter(supplier),
                    "SQL" => new SQLSupplierAdapter(_configuration.GetConnectionString("SqlServer")),
                    "HTTPAPI" => new HttpApiSupplierAdapter(supplier.SourcePath),
                    "CSV" => new CSVSupplierAdapter(supplier.SourcePath),
                    _ => throw new NotImplementedException($"Adapter for {supplier.DataSourceType} not implemented.")
                };
                adapters.Add(adapter);
            }

            return adapters;
        }

        private IProductAdapter CreateJsonAdapter(SupplierConfig supplier)
        {
            var adapterType = typeof(JSONSupplierAdapter<>).MakeGenericType(supplier.DtoTypeResolved);
            return (IProductAdapter)Activator.CreateInstance(adapterType, supplier, _mapper);
        }
    }
}
