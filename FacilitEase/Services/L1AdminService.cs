using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Services
{
    public class L1AdminService:IL1AdminService
    {
        private readonly AppDbContext _context;
        public L1AdminService(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<ProfileData> GetSuggestions(string text)
        {
            text=text.ToLower();
            var suggestions = _context.Employee
                    .Where(employee =>
                        employee.FirstName.ToLower().Contains(text) ||
                        employee.LastName.ToLower().Contains(text)
                        )
                    .Select(employee => new ProfileData
                        {
                            EmpId = employee.Id,
                            EmployeeFirstName = employee.FirstName,
                            EmployeeLastName = employee.LastName,
                            JobTitle = _context.Position
                                .Where(p => p.Id == _context.EmployeeDetail
                                .Where(ed => ed.EmployeeId == employee.Id)
                                .Select(ed => ed.PositionId)
                                .FirstOrDefault())
                                .Select(p => p.PositionName)
                                .FirstOrDefault() ?? "",
                                Username = _context.User
                                .Where(u => u.EmployeeId == employee.Id)
                                .Select(u => u.Email)
                                .FirstOrDefault() ?? ""
                    })
                    .OrderBy(profileData => profileData.EmployeeFirstName) // Sort by EmployeeFirstName
                    .ToList();
        return suggestions;
        }
        public IEnumerable<string> GetRoles()
        {
            var roles = from r in _context.UserRole
                        select r.UserRoleName;
            return roles;
        }
        public void AssignRole(AssignRole assignRole)
        {
            int empId= assignRole.EmpId;
            string roleName = assignRole.RoleName;
            UserRoleMapping roleMapping=new UserRoleMapping();
            roleMapping.UserId = (from u in _context.User
                                 where u.EmployeeId == empId
                                 select u.Id).FirstOrDefault();
            roleMapping.UserRoleId = (from r in _context.UserRole
                                      where r.UserRoleName == roleName
                                      select r.Id).FirstOrDefault();
            DateTime currentDateTime = DateTime.Now;
            roleMapping.CreatedDate = currentDateTime;
            roleMapping.UpdatedDate = currentDateTime;
            roleMapping.CreatedBy = 1;
            roleMapping.UpdatedBy=1;


            _context.User_Role_Mapping.Add(roleMapping);
            _context.SaveChanges();
            
        }
    }
}
