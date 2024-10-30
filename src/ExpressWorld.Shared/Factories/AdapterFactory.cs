using AutoMapper;
using ExpressWorld.Shared.Adapters;
using ExpressWorld.Shared.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ExpressWorld.Shared.Factories
{
    //Consider Using an Abstract Factory with Dependency Injection
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
                //Voilates Open/Closed Principle
                //ties the factory class to each specific adapter implementation
                // This is less flexible and makes it difficult to add new adapters without modifying the factory class each time
                IProductAdapter adapter = supplier.DataSourceType.ToUpper() switch
                {
                    "JSON" => CreateJsonAdapter(supplier),
                    "SQL" => new SQLSupplierAdapter(_configuration.GetConnectionString("SqlServer")),
                    "HTTPAPI" => new HttpApiSupplierAdapter(supplier.SourcePath, null),
                    "CSV" => new CSVSupplierAdapter(supplier.SourcePath),
                    _ => throw new NotImplementedException($"Adapter for {supplier.DataSourceType} not implemented.")
                };
                adapters.Add(adapter);
            }

            return adapters;
        }

        // Activator.CreateInstance introduces runtime reflection, which can be error-prone, slower, and difficult to test. Using a more type-safe method would improve performance and readability.

        private IProductAdapter CreateJsonAdapter(SupplierConfig supplier)
        {
            // Ensure that DtoTypeResolved is checked before using it
            if (supplier.DtoTypeResolved == null)
            {
                throw new InvalidOperationException($"Could not resolve DTO type '{supplier.DtoType}'.");
            }
            //
            var adapterType = typeof(JSONSupplierAdapter<>).MakeGenericType(supplier.DtoTypeResolved);
            return (IProductAdapter)Activator.CreateInstance(adapterType, supplier, _mapper);
        }
    }
}
