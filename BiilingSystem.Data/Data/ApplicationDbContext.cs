using BillingSystem.Contracts.Models.Invoice;
using BillingSystem.Models.Products;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace BillingSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<UserInvoiceModel> UserInvoices { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>()
               .Property(p => p.Price)
               .HasColumnType("decimal(18,2)");
            base.OnModelCreating(builder);
            SeedRole(builder);
        }

        private void SeedRole(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                 new IdentityRole() { Name = "Agent", ConcurrencyStamp = "2", NormalizedName = "Agent" },
                  new IdentityRole()
                  {
                      Name = "Customer",
                      ConcurrencyStamp = "3",
                      NormalizedName = "Customer"
                  }
                );
        }
    }
}
