using LaptopRental.Models;
using Microsoft.EntityFrameworkCore;

namespace LaptopRental.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Laptop> Laptops { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _ = optionsBuilder.UseSqlServer(connectionString: "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true");
        }

    }
}
