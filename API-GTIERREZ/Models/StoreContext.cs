using Microsoft.EntityFrameworkCore;

namespace API_GTIERREZ.Models
{
    public class StoreContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Detail> Details { get; set; }

        public StoreContext(DbContextOptions<StoreContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Customer)
                .WithMany()
                .HasForeignKey(i => i.CustomerID);

            modelBuilder.Entity<Detail>()
                .HasOne(d => d.Invoice)
                .WithMany(i => i.Details)
                .HasForeignKey(d => d.InvoiceID);

            modelBuilder.Entity<Detail>()
                .HasOne(d => d.Product)
                .WithMany()
                .HasForeignKey(d => d.ProductID);

           
            modelBuilder.Entity<Customer>().HasQueryFilter(c => c.Active);
            modelBuilder.Entity<Product>().HasQueryFilter(p => p.Active);
            modelBuilder.Entity<Invoice>().HasQueryFilter(i => i.Active);
            modelBuilder.Entity<Detail>().HasQueryFilter(d => d.Active);
        }
    }
}
