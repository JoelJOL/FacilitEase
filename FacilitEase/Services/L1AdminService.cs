using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Services
{
    public class L1AdminService : IL1AdminService
    {
        private readonly AppDbContext _context;

        public L1AdminService(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// To get the suggestions of details of employees that are having similar names to the string contained in text
        /// </summary>
        /// <param name="text">Search parameter that is entered in the searchbar</param>
        /// <returns>All the details of employees that are having similar names to string in text</returns>
        public IEnumerable<ProfileData> GetSuggestions(string text)
        {
            //Converting text into lowercase to avoid case sensitivity while comparing data from databse and text
            text = text.ToLower();

            //Select the employees that have similar names to the text
            //Here ProfileData is an ApiModel to stroe the required data
            var suggestions = _context.TBL_EMPLOYEE
                    .Where(employee =>
                        employee.FirstName.ToLower().Contains(text) ||
                        employee.LastName.ToLower().Contains(text)
                        )
                    .Select(employee => new ProfileData
                    {
                        EmpId = employee.Id,
                        EmployeeFirstName = employee.FirstName,
                        EmployeeLastName = employee.LastName,
                        JobTitle = _context.TBL_POSITION
                                .Where(p => p.Id == _context.TBL_EMPLOYEE_DETAIL
                                .Where(ed => ed.EmployeeId == employee.Id)
                                .Select(ed => ed.PositionId)
                                .FirstOrDefault())
                                .Select(p => p.PositionName)
                                .FirstOrDefault() ?? "",
                        Username = _context.TBL_USER
                                .Where(u => u.EmployeeId == employee.Id)
                                .Select(u => u.Email)
                                .FirstOrDefault() ?? ""
                    })
                    .OrderBy(profileData => profileData.EmployeeFirstName) // Sort by EmployeeFirstName
                    .ToList();
            return suggestions;
        }
        /// <summary>
        /// To get all the roles of an employee that are available to him
        /// </summary>
        /// <param name="id">User id of the user whose assignable roles must be fetched</param>
        /// <returns>All assignable roles of a user</returns>
        public IEnumerable<string> GetRoles(int id)
        {
            //Get the current roles that the employee roles
            var mappedRoles = from r in _context.TBL_USER_ROLE
                              join ur in _context.TBL_USER_ROLE_MAPPING on r.Id equals ur.UserRoleId
                              join u in _context.TBL_USER on ur.UserId equals u.Id
                              where u.Id == id
                              select r.UserRoleName;

            //Get all the possible roles from database
            var allRoles = _context.TBL_USER_ROLE.Select(r => r.UserRoleName);

            //Get the roles that can be assigned to the user
            var roles = allRoles.Except(mappedRoles);

            return roles;
        }
        /// <summary>
        /// Assigning a role to an employee
        /// </summary>
        /// <param name="assignRole">An apiModel that consists of the employeeId and the role name that must be assigned</param>
        public void AssignRole(AssignRole assignRole)
        {
            //AssignRole is an ApiModel that has data - employeeid and rolename that must be assigned to the user
            int empId = assignRole.EmpId;
            string roleName = assignRole.RoleName;
            TBL_USER_ROLE_MAPPING roleMapping = new TBL_USER_ROLE_MAPPING();
            roleMapping.UserId = (from u in _context.TBL_USER
                                  where u.EmployeeId == empId
                                  select u.Id).FirstOrDefault();
            roleMapping.UserRoleId = (from r in _context.TBL_USER_ROLE
                                      where r.UserRoleName == roleName
                                      select r.Id).FirstOrDefault();
            DateTime currentDateTime = DateTime.Now;
            roleMapping.CreatedDate = currentDateTime;
            roleMapping.UpdatedDate = currentDateTime;
            roleMapping.CreatedBy = 1;
            roleMapping.UpdatedBy = 1;

            //Adding a new row into the UserRoleMapping table with the empId and RoleId
            _context.TBL_USER_ROLE_MAPPING.Add(roleMapping);
            _context.SaveChanges();
        }
    }
}