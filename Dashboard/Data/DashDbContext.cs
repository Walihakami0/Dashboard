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

        public DbSet<Customer> Customer { get; set; }

        public DbSet<ProductDetails> ProductDetails { get; set; }

        public DbSet<Invoice> Invoice { get; set; }

        public DbSet<Cart> Cart { get; set; }
        public DbSet<Payments> Payments { get; set; }

        
    }
}
