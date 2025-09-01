using ConditionalFluentValidationSetup.Models;
using Microsoft.EntityFrameworkCore;

namespace ConditionalFluentValidationSetup.Data
{
    public class ECommerceDbContext : DbContext
    {
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>().HasData(
                new Customer { CustomerId = 1, Name = "John Doe" },
                new Customer { CustomerId = 2, Name = "Jane Smith" },
                new Customer { CustomerId = 3, Name = "Emily Johnson" }
            );
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }

    }
}
