
using ExpressWorld.Core.Entities;
using ExpressWorld.Core.Interfaces;
using ExpressWorld.Infrastructure.Persistence;

namespace ExpressWorld.Infrastructure.Adapters
{
    public class SqlServerAdapter : ISupplierAdapter
    {
        private readonly ProductDbContext _dbContext;

        public SqlServerAdapter(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> FetchProductsAsync()
        {
            // Query the products table and return as a list
            // return await _dbContext.Products.ToListAsync();

            return new List<Product>();
        }
    }
}
