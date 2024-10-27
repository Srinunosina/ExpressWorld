using CsvHelper;
using ExpressWorld.Core.Entities;
using System.Globalization;

namespace ExpressWorld.Shared.Adapters
{
    public class CSVSupplierAdapter : IProductAdapter
    {
        private readonly string _filePath;

        public CSVSupplierAdapter(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<IEnumerable<Product>> LoadProductsAsync(CancellationToken cancellationToken)
        {
            var products = new List<Product>();
            using var reader = new StreamReader(_filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            products = csv.GetRecords<Product>().ToList();
            return await Task.FromResult(products);
        }
    }
}
