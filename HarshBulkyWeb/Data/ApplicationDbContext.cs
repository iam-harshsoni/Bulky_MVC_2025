using Microsoft.EntityFrameworkCore;

namespace HarshBulkyWeb.Data
{
   //Primary Constructor
   public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {

    }
}
