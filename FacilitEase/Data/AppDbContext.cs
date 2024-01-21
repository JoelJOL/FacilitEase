using FacilitEase.Models.EntityModels;
using Microsoft.EntityFrameworkCore;
namespace FacilitEase.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TBL_EMPLOYEE> Employees { get; set; }

    }
}
