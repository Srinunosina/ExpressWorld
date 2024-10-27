using ExpressWorld.Core.Entities;
using ExpressWorld.Core.Interfaces;
using System.Text.Json;

namespace ExpressWorld.Infrastructure.Adapters
{
    public class JsonFileAdapter : ISupplierAdapter
    {
        private readonly string _filePath;

        public JsonFileAdapter(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<IEnumerable<Product>> FetchProductsAsync(CancellationToken )
        {
            using var stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
            return await JsonSerializer.DeserializeAsync<List<Product>>(stream) ?? new List<Product>();
        }
    }

}
