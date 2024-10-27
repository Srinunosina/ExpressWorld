using ExpressWorld.Core.Entities;
using ExpressWorld.Core.Interfaces;

namespace ExpressWorld.Infrastructure.Adapters
{
    public class CsvFileAdapter : ISupplierAdapter
    {
        private readonly string _filePath;

        public CsvFileAdapter(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<IEnumerable<Product>> FetchProductsAsync()
        {
            var products = new List<Product>();
            using var reader = new StreamReader(_filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            products = csv.GetRecords<Product>().ToList();
            return await Task.FromResult(products);
        }
    }

}
