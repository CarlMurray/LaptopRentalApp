using LaptopRental.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRental.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Brand> Brands {  get; set; }
        public DbSet<Laptop> Laptops { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString:"Server=(localdb)\\MSSQLLocalDB;Integrated Security=true");
        }

    }
}
