
using Dapper;
using ExpressWorld.Core.Entities;
using Microsoft.Data.SqlClient;

namespace ExpressWorld.Shared.Adapters
{
    public class SQLSupplierAdapter : IProductAdapter
    {
        private readonly string _connectionString;
        public SQLSupplierAdapter(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<IEnumerable<Product>> LoadProductsAsync(CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT Id, Name, Description, Price, Category FROM Products";
                var products = await connection.QueryAsync<Product>(query);

                return products;
            }
        }
    }
}
