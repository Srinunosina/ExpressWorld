using AutoMapper;
using ExpressWorld.Shared.Adapters;
using ExpressWorld.Shared.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ExpressWorld.Shared.Factories
{   
    public class AdapterFactory
    {
        private readonly IOptionsMonitor<List<SupplierConfig>> _supplierConfigs;
        private readonly IMapper _mapper;
        IConfiguration _configuration;

        public AdapterFactory(IOptionsMonitor<List<SupplierConfig>> supplierConfigs, IMapper mapper, IConfiguration configuration)
        {
            _supplierConfigs = supplierConfigs;
            _mapper = mapper;
            _configuration = configuration;
        }

        /// <summary>
        /// Creates and returns a collection of product adapters based on the supplier configurations.
        /// </summary>
        /// <returns>Returns list of ProductAdapters that containing instances of various implementations (JSON, API, DB, CSV).</returns>
        /// <exception cref="NotImplementedException">Thrown when a data source type does not have a corresponding adapter implemented.</exception>
        public IEnumerable<IProductAdapter> CreateAdapters()
        {
            var adapters = new List<IProductAdapter>();
            var suppliers = _supplierConfigs.CurrentValue; // Access current supplier configurations

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
            // Ensure that DtoTypeResolved is checked before using it
            if (supplier.DtoTypeResolved == null)
            {
                throw new InvalidOperationException($"Could not resolve DTO type '{supplier.DtoType}'.");
            }

            var adapterType = typeof(JSONSupplierAdapter<>).MakeGenericType(supplier.DtoTypeResolved);
            return (IProductAdapter)Activator.CreateInstance(adapterType, supplier, _mapper);
        }
    }
}
