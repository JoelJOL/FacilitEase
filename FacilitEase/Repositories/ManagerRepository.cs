using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.Repositories;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Repository for managing data related to managers.
/// </summary>
public class ManagerRepository : IManagerRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Initializes a new instance of the ManagerRepository class.
    /// </summary>
    /// <param name="context">The application's database context.</param>
    public ManagerRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Asynchronously gets a list of all managers.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of Managers.</returns>
    public async Task<IEnumerable<ManagerAPI>> GetManagersAsync()
    {
        try
        {
            var managers = await (from userRole in _context.UserRole
                                  where userRole.UserRoleName == "Manager"
                                  join userRoleMapping in _context.User_Role_Mapping on userRole.Id equals userRoleMapping.UserRoleId
                                  join user in _context.User on userRoleMapping.UserId equals user.Id
                                  join employee in _context.Employee on user.Id equals employee.Id
                                  select new ManagerAPI
                                  {
                                      ManagerId = employee.Id,
                                      ManagerName = $"{employee.FirstName} {employee.LastName}"
                                  }).ToListAsync();

            return managers;
        }
        catch (Exception ex)
        {
            // Log the exception details
            Console.WriteLine($"Error in GetManagersAsync: {ex.Message}");
            return Enumerable.Empty<ManagerAPI>();
        }
    }
}
