using FacilitEase.Models.EntityModels;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using System.Net.Sockets;
namespace FacilitEase.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TBL_EMPLOYEE> TBL_EMPLOYEE { get; set; }
        public DbSet<TBL_PRIORITY> TBL_PRIORITY { get; set; }
        public DbSet<TBL_DEPARTMENT> TBL_DEPARTMENT { get; set; }
        public DbSet<TBL_STATUS> TBL_STATUS { get; set; }
        public DbSet<TBL_CATEGORY> TBL_CATEGORY { get; set; }
        public DbSet<TBL_USER> TBL_USER { get; set; }
        public DbSet<TBL_USER_ROLE> TBL_USER_ROLE { get; set; }
        public DbSet<TBL_USER_ROLE_MAPPING> TBL_USER_ROLE_MAPPING { get; set; }
        public DbSet<TBL_TICKET> TBL_TICKET { get; set; }
        public DbSet<TBL_EMPLOYEE_DETAIL> TBL_EMPLOYEE_DETAIL { get; set; }
        public DbSet<TBL_DOCUMENT> TBL_DOCUMENT { get; set; }
        public DbSet<TBL_LOCATION> TBL_LOCATION { get; set; }
        public DbSet<TBL_PROJECT_EMPLOYEE_MAPPING> TBL_PROJECT_EMPLOYEE_MAPPING { get; set; }
        public DbSet<TBL_PROJECT> TBL_PROJECT { get; set; }
        public DbSet<TBL_TICKET_TRACKING> TBL_TICKET_TRACKING { get; set; }
        public DbSet<TBL_TICKET_ASSIGNMENT> TBL_TICKET_ASSIGNMENT { get; set; }
        public DbSet<TBL_LOGIN> TBL_LOGIN { get; set; }
        public DbSet<TBL_POSITION> TBL_POSITION { get; set; }
        public DbSet<TBL_BUDGET_CODE_GENERATION> TBL_BUDGET_CODE_GENERATION { get; set; }
        public DbSet<TBL_PROJECT_CODE_GENERATION> TBL_PROJECT_CODE_GENERATION { get; set; }
        public DbSet<TBL_ASSET_TYPE> TBL_ASSET_TYPE { get; set; }
        public DbSet<TBL_ASSET_STATUS> TBL_ASSET_STATUS { get; set; }
        public DbSet<TBL_ASSET> TBL_ASSET { get; set; }
        public DbSet<TBL_ASSET_EMPLOYEE_MAPPING> TBL_ASSET_EMPLOYEE_MAPPING { get; set; }
        public DbSet<TBL_COMMENT> TBL_COMMENT { get; set; }
        public DbSet<TBL_NOTIFICATION> TBL_NOTIFICATION { get; set; }
    }

}
