using FacilitEase.Models.EntityModels;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using System.Net.Sockets;
namespace FacilitEase.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TBL_EMPLOYEE> Employee { get; set; }
        public DbSet<TBL_PRIORITY> Priority { get; set; }
        public DbSet<TBL_DEPARTMENT> Department { get; set; }
        public DbSet<TBL_STATUS> Status { get; set; }
        public DbSet<TBL_CATEGORY> Category { get; set; }
        public DbSet<TBL_USER> User { get; set; }
        public DbSet<TBL_USER_ROLE> UserRole { get; set; }
        public DbSet<TBL_USER_ROLE_MAPPING> User_Role_Mapping { get; set; }
        public DbSet<TBL_TICKET> Ticket { get; set; }
        public DbSet<TBL_EMPLOYEE_DETAIL> EmployeeDetail { get; set; }
        public DbSet<TBL_DOCUMENT> Document { get; set; }
        public DbSet<TBL_LOCATION> Location { get; set; }
        public DbSet<TBL_PROJECT_EMPLOYEE_MAPPING> ProjectEmployeeMapping { get; set; }
        public DbSet<TBL_PROJECT> Project { get; set; }
        public DbSet<TBL_TICKET_TRACKING> TicketTracking { get; set; }
        public DbSet<TBL_TICKET_ASSIGNMENT> TicketAssignment { get; set; }
        public DbSet<TBL_LOGIN> Login { get; set; }
        public DbSet<TBL_POSITION> Position { get; set; }
        public DbSet<TBL_BUDGET_CODE_GENERATION> BudgetCodeGeneration { get; set; }
        public DbSet<TBL_PROJECT_CODE_GENERATION> ProjectCodeGeneration { get; set; }
        public DbSet<TBL_ASSET_TYPE> AssetType { get; set; }
        public DbSet<TBL_ASSET_STATUS> AssetStatus { get; set; }
        public DbSet<TBL_ASSET> Asset { get; set; }
        public DbSet<TBL_ASSET_EMPLOYEE_MAPPING> AssetEmployeeMapping { get; set; }
        public DbSet<TBL_COMMENT> Comment { get; set; }
        public DbSet<TBL_NO> Notification { get; set; }
    }

}
