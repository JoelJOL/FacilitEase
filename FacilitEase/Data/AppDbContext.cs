using FacilitEase.Models.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace FacilitEase.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TBL_CATEGORY> TBL_CATEGORY { get; set; }
        public DbSet<TBL_DEPARTMENT> TBL_DEPARTMENT { get; set; }
        public DbSet<TBL_TICKET> TBL_TICKET { get; set; }
        public DbSet<TBL_PRIORITY> TBL_PRIORITY { get; set; }
    }

}
