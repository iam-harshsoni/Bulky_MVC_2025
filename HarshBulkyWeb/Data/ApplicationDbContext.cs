using HarshBulkyWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace HarshBulkyWeb.Data
{
    //Primary Constructor
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Category> Categories { get; set; }
    }
}