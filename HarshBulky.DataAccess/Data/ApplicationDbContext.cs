using HarshBulky.Models;
using Microsoft.EntityFrameworkCore;
using Bogus;

namespace HarshBulky.DataAccess.Data
{
    //Primary Constructor
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(

                new Category { CategoryId = 1, Name = "Action", DisplayOrder = 1 },
                new Category { CategoryId = 2, Name = "SciFi", DisplayOrder = 2 },
                new Category { CategoryId = 3, Name = "History", DisplayOrder = 3 }

                );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Awesome Product",
                    Author = "John Doe",
                    Description = "A static description for testing.",
                    ISBN = "1234567890123",
                    ListPrice = 100.0,
                    Price = 95.0,
                    Price50 = 50.0,
                    Price100 = 80.0
                },
                new Product
                {
                    Id = 2,
                    Title = "Great Gadget",
                    Author = "Jane Smith",
                    Description = "Another static description for testing.",
                    ISBN = "9876543210987",
                    ListPrice = 120.0,
                    Price = 110.0,
                    Price50 = 60.0,
                    Price100 = 96.0
                },
                new Product
                {
                    Id = 3,
                    Title = "Super Widget",
                    Author = "Emily Johnson",
                    Description = "Yet another static description for testing.",
                    ISBN = "3216549870123",
                    ListPrice = 150.0,
                    Price = 140.0,
                    Price50 = 75.0,
                    Price100 = 120.0
                },
                new Product
                {
                    Id = 4,
                    Title = "Amazing Device",
                    Author = "Michael Brown",
                    Description = "A new static description for testing purposes.",
                    ISBN = "4567891230987",
                    ListPrice = 80.0,
                    Price = 75.0,
                    Price50 = 40.0,
                    Price100 = 64.0
                },
                new Product
                {
                    Id = 5,
                    Title = "Ultimate Tool",
                    Author = "Sarah White",
                    Description = "Final static description for testing.",
                    ISBN = "7890123456789",
                    ListPrice = 200.0,
                    Price = 190.0,
                    Price50 = 100.0,
                    Price100 = 160.0
                }
            );
        }
    }
}