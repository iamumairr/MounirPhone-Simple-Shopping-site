using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MounirPhone.Models;

namespace MounirPhone.Data
{
    public class AppDbContext:IdentityDbContext 
    {
        public AppDbContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<ContactDetails> ContactDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}