using AutoMapper;
using ExpressWorld.Shared.Adapters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressWorld.Shared.Factories
{
    //public class AdapterFactory
    //{
    //    private readonly IConfiguration _configuration;
    //    private readonly IMapper _mapper;

    //    public AdapterFactory(IConfiguration configuration, IMapper mapper)
    //    {
    //        _configuration = configuration;
    //        _mapper = mapper;
    //    }

    //    public IEnumerable<IProductAdapter> CreateAdapters()
    //    {
    //        var adapters = new List<IProductAdapter>();
    //        var suppliers = _configuration.GetSection("Suppliers").Get<IEnumerable<SupplierConfig>>();

    //        foreach (var supplier in suppliers)
    //        {
    //            IProductAdapter adapter = supplier.DataSourceType switch
    //            {
    //                "JSON" => CreateJsonAdapter(supplier),
    //                "SQL" => new SQLSupplierAdapter(_configuration.GetConnectionString("SqlServer")),
    //                "HTTPAPI" => new HttpApiSupplierAdapter(supplier.SourcePath),
    //                "CSV" => new CSVSupplierAdapter(supplier.SourcePath),
    //                _ => throw new NotImplementedException($"Adapter for {supplier.DataSourceType} not implemented.")
    //            };
    //            adapters.Add(adapter);
    //        }

    //        return adapters;
    //    }

    //    private IProductAdapter CreateJsonAdapter(SupplierConfig supplier)
    //    {
    //        var adapterType = typeof(JSONSupplierAdapter<>).MakeGenericType(supplier.DtoTypeResolved);
    //        return (IProductAdapter)Activator.CreateInstance(adapterType, supplier.SourcePath, _mapper);
    //    }
    //}

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
                    "HTTPAPI" => new HttpApiSupplierAdapter(supplier.SourcePath), // Pass SourcePath directly
                    "CSV" => new CSVSupplierAdapter(supplier.SourcePath), // Pass SourcePath directly
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
          //  return (IProductAdapter)Activator.CreateInstance(adapterType, supplier.SourcePath, supplier.RootPropertyName, _mapper);
        }
    }


    public class SupplierConfig
    {
        public string Name { get; set; }
        public string DataSourceType { get; set; }
        public string SourcePath { get; set; }
        public string DtoType { get; set; }
        public string RootPropertyName { get; set; }        
        public Type DtoTypeResolved => Type.GetType(DtoType);
    }

    //public class SupplierConfig
    //{
    //    public string Name { get; set; }
    //    public string DataSourceType { get; set; } // JSON, CSV, HTTPAPI, etc.
    //    public string SourcePath { get; set; } // Could be a file path or API endpoint
    //}
    //public class SupplierConfig
    //{
    //    public string Name { get; set; }
    //    public string DataSourceType { get; set; }
    //    public string SourcePath { get; set; }
    //    public string DtoTypeName { get; set; } // Configuration property name

    //    public Type DtoType => Type.GetType(DtoTypeName);
    //}
}
