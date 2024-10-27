using ExpressWorld.Core.Entities;
using ExpressWorld.Core.Interfaces;
using System.Text.Json;

namespace ExpressWorld.Infrastructure.Adapters
{
    public class HttpApiAdapter : ISupplierAdapter
    {
        private readonly string _apiEndpoint;
        private readonly HttpClient _httpClient;

        public HttpApiAdapter(string apiEndpoint)
        {
            _apiEndpoint = apiEndpoint;
            _httpClient = new HttpClient();
        }

        public async Task<IEnumerable<Product>> FetchProductsAsync()
        {
            var response = await _httpClient.GetStringAsync(_apiEndpoint);
            return JsonSerializer.Deserialize<List<Product>>(response) ?? new List<Product>();
        }
    }

}
