
using ExpressWorld.Core.Interfaces;
using ExpressWorld.Infrastructure.Adapters;
using ExpressWorld.Infrastructure.Configurations;

namespace ExpressWorld.Infrastructure.Factories
{
    public class SupplierFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public SupplierFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ISupplierAdapter CreateAdapter(SupplierConfig config)
        {
            return config.DataSourceType switch
            {
                "JsonFile" => new JsonFileAdapter(config.SourcePath),
                "HttpApi" => new HttpApiAdapter(config.ApiEndpoint),
                "CsvFile" => new CsvFileAdapter(config.SourcePath),
                "SqlServer" => _serviceProvider.GetRequiredService<SqlServerAdapter>(),
                _ => throw new NotImplementedException($"No adapter for data source type {config.DataSourceType}"),
            };
        }
    }
}
