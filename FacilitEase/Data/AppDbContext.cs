using FacilitEase.Models.EntityModels;
using Microsoft.EntityFrameworkCore;
namespace FacilitEase.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TBL_STATUS> TBL_STATUS { get; set; }
        public DbSet<TBL_TICKET> TBL_TICKET { get; set; }
    }
}
