using System.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyWebRazor_Temp.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Category> Categories {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(

                new Category { CategoryId = 1, Name = "SciFi", DisplayOrder = 1 },
                 new Category { CategoryId = 2, Name = "SciFi", DisplayOrder = 2 },
                new Category { CategoryId = 3, Name = "History", DisplayOrder = 3 }
            );
        }
    }
}
