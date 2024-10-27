using ExpressWorld.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpressWorld.Infrastructure.Persistence
{
    public class ProductDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {
        }
    }
}
