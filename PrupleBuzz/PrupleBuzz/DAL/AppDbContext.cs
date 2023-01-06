using Microsoft.EntityFrameworkCore;
using PrupleBuzz.Models;

namespace PrupleBuzz.DAL
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        public DbSet<Slider> Slider { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceImage> Images { get; set; }
        public DbSet<Work> Works { get; set; }
        public DbSet<WorkImage> WorkImages { get; set; }

    }
}
