using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EShop.MVC.Models;

namespace EShop.MVC.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public DbSet<Product> Products { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) 
        { 
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            base.OnModelCreating(builder);

          
            builder.Entity<Product>()
                   .Property(p => p.Price)
                   .HasColumnType("decimal(18,2)");
        }
    }
}
