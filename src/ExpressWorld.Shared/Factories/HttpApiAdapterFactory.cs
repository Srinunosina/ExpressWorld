using ExpressWorld.Shared.FactoryMethod;
using ExpressWorld.Shared.Adapters;
using ExpressWorld.Shared.Configurations;

namespace ExpressWorld.Shared.Factories
{
    public class HttpApiAdapterFactory : IAdapterFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpApiAdapterFactory(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public bool CanHandle(string dataSourceType) => dataSourceType.Equals("HTTPAPI", StringComparison.OrdinalIgnoreCase);

        public IProductAdapter CreateAdapter(SupplierConfig supplierConfig)
        {
            if (string.IsNullOrWhiteSpace(supplierConfig.SourcePath))
            {
                throw new ArgumentException("API endpoint is required for HTTPAPI adapter.");
            }

            // Use IHttpClientFactory to manage HttpClient instances and avoid socket exhaustion
            var httpClient = _httpClientFactory.CreateClient();
            return new HttpApiSupplierAdapter(supplierConfig.SourcePath, httpClient);
        }
    }

}
