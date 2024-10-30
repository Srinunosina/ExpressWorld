using ExpressWorld.Core.Entities;
using System.Text.Json;

namespace ExpressWorld.Shared.Adapters
{
    public class HttpApiSupplierAdapter : IProductAdapter
    {
        private readonly string _apiEndpoint;
        private readonly HttpClient _httpClient;

        public HttpApiSupplierAdapter(string apiEndpoint, HttpClient httpClient)
        {
            _apiEndpoint = apiEndpoint;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Product>> LoadProductsAsync(CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetStringAsync(_apiEndpoint, cancellationToken);
            return JsonSerializer.Deserialize<List<Product>>(response) ?? new List<Product>();
        }
    }
}
