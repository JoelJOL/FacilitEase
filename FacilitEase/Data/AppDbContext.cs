using FacilitEase.Models.EntityModels;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using System.Net.Sockets;
namespace FacilitEase.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employee { get; set; }
        public DbSet<Priority> Priority { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<UserRoleMapping> User_Role_Mapping { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<EmployeeDetail> EmployeeDetail { get; set; }
        public DbSet<Document> Document { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<ProjectEmployeeMapping> ProjectEmployeeMapping { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<TicketTracking> TicketTracking { get; set; }
        public DbSet<TicketAssignment> TicketAssignment { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<Position> Position { get; set; }
        public DbSet<BudgetCodeGeneration> BudgetCodeGeneration { get; set; }
        public DbSet<ProjectCodeGeneration> ProjectCodeGeneration { get; set; }
        public DbSet<AssetType> AssetType { get; set; }
        public DbSet<AssetStatus> AssetStatus { get; set; }
        public DbSet<Asset> Asset { get; set; }
        public DbSet<AssetEmployeeMapping> AssetEmployeeMapping { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Notification> Notification { get; set; }
    }

}
