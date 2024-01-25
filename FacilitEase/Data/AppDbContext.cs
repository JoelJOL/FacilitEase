using FacilitEase.Models.EntityModels;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
namespace FacilitEase.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<TBL_TICKET_ASSIGNMENT> TBL_TICKET_ASSIGNMENT { get; set; }
        public DbSet<TBL_POSITION> TBL_POSITION { get; set; }
        public DbSet<TBL_EMPLOYEE> TBL_EMPLOYEE{ get; set; }
        public DbSet<TBL_USER> TBL_USER { get; set; }
        public DbSet<TBL_EMPLOYEE_DETAIL> TBL_EMPLOYEE_DETAIL { get; set; }

    }
}
