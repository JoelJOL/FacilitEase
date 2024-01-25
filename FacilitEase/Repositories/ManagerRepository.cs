using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.Repositories;
using Microsoft.EntityFrameworkCore;

public class ManagerRepository : IManagerRepository
{
    private readonly AppDbContext _context;

    public ManagerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ManagerAPI>> GetManagersAsync()
    {
        try
        {
            var managers = await (from userRole in _context.TBL_USER_ROLE
                                  where userRole.UserRoleName == "Manager"
                                  join userRoleMapping in _context.TBL_USER_ROLE_MAPPING on userRole.Id equals userRoleMapping.UserRoleId
                                  join user in _context.TBL_USER on userRoleMapping.UserId equals user.Id
                                  join employee in _context.TBL_EMPLOYEE on user.Id equals employee.Id
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
