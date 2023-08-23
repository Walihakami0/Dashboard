using Dashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Data
{
    public class DashDbContext: DbContext
    {
        public DashDbContext(DbContextOptions<DashDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Product { get; set; }

        public DbSet<Customer> customer { get; set; }

        public DbSet<ProductDetails> productDetails { get; set; }

        public DbSet<Invoice> invoice { get; set; }
    }
}
