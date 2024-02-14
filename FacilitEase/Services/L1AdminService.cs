using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using System.Linq.Dynamic.Core;
using System.Net.Sockets;
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

        public ManagerTicketResponse<TicketApiModel> GetEscalatedTickets(int userId, int pageIndex, int pageSize, string sortField, string sortOrder, string searchQuery)
        {
            // Step 1: Retrieve DepartmentId based on UserId
            var departmentId = _context.TBL_EMPLOYEE_DETAIL
                .Where(employeeDetail => employeeDetail.EmployeeId == _context.TBL_USER
                    .Where(user => user.Id == userId)
                    .Select(user => user.EmployeeId)
                    .FirstOrDefault())
                .Select(employeeDetail => employeeDetail.DepartmentId)
                .FirstOrDefault();

            // Step 2: Get Categories corresponding to DepartmentId
            var categoriesForDepartment = _context.TBL_CATEGORY
                .Where(category => category.DepartmentId == departmentId)
                .Select(category => category.Id)
                .ToList();

            // Step 3: Get the StatusId for "Escalated" from TBL_STATUS
            var escalatedStatusId = _context.TBL_STATUS
                .Where(status => status.StatusName == "Escalated")
                .Select(status => status.Id)
                .FirstOrDefault();

            // Step 4: Get the UserId of L3Admin role
            var l3AdminRoleId = _context.TBL_USER_ROLE
     .Where(role => role.UserRoleName == "L2Admin")
     .Select(role => role.Id)
     .FirstOrDefault();

            var l3AdminEmployeeIds = _context.TBL_USER_ROLE_MAPPING
                .Where(mapping => mapping.UserRoleId == l3AdminRoleId)
                .Join(_context.TBL_USER,
                    mapping => mapping.UserId,
                    user => user.Id,
                    (mapping, user) => user.EmployeeId)
                .ToList();

            // Step 5: Filter escalated tickets for L3Admin assignedTo
            var escalatedTicketsQuery = _context.TBL_TICKET
                .Where(ticket => ticket.AssignedTo != null)
                .Where(ticket => ticket.StatusId == escalatedStatusId)
                .Where(ticket => categoriesForDepartment.Contains(ticket.CategoryId))
                .Where(ticket => l3AdminEmployeeIds.Contains(ticket.AssignedTo.Value))
                .Where(ticket => string.IsNullOrEmpty(searchQuery) || ticket.TicketName.Contains(searchQuery))
                .Select(ticket => new
                {
                    Id = ticket.Id,
                    TicketName = ticket.TicketName,
                    RaisedBy = _context.TBL_EMPLOYEE
                        .Where(employee => employee.Id == _context.TBL_USER
                            .Where(user => user.Id == ticket.UserId)
                            .Select(user => user.EmployeeId)
                            .FirstOrDefault())
                        .Select(employee => $"{employee.FirstName} {employee.LastName}")
                        .FirstOrDefault(),
                    SubmittedDate = ticket.SubmittedDate,
                    Priority = _context.TBL_PRIORITY
                        .Where(priority => priority.Id == ticket.PriorityId)
                        .Select(priority => priority.PriorityName)
                        .FirstOrDefault(),
                    Status = _context.TBL_STATUS
                        .Where(status => status.Id == ticket.StatusId)
                        .Select(status => status.StatusName)
                        .FirstOrDefault(),
                    AssignedTo = _context.TBL_EMPLOYEE
                        .Where(employee => employee.Id == ticket.AssignedTo)
                        .Select(employee => $"{employee.FirstName} {employee.LastName}")
                        .FirstOrDefault(),
                    Department = _context.TBL_USER
                        .Where(user => user.Id == ticket.UserId)
                        .Select(user => _context.TBL_EMPLOYEE
                            .Where(employee => employee.Id == user.EmployeeId)
                            .Select(employee => _context.TBL_EMPLOYEE_DETAIL
                                .Where(employeeDetail => employeeDetail.Id == employee.Id)
                                .Select(employeeDetail => _context.TBL_DEPARTMENT
                                    .Where(department => department.Id == employeeDetail.DepartmentId)
                                    .Select(department => department.DeptName)
                                    .FirstOrDefault())
                                .FirstOrDefault())
                            .FirstOrDefault())
                        .FirstOrDefault(),
                    Location = _context.TBL_USER
                        .Where(user => user.Id == ticket.UserId)
                        .Select(user => _context.TBL_EMPLOYEE
                            .Where(employee => employee.Id == user.EmployeeId)
                            .Select(employee => _context.TBL_EMPLOYEE_DETAIL
                                .Where(employeeDetail => employeeDetail.Id == employee.Id)
                                .Select(employeeDetail => _context.TBL_LOCATION
                                    .Where(location => location.Id == employeeDetail.LocationId)
                                    .Select(location => location.LocationName)
                                    .FirstOrDefault())
                                .FirstOrDefault())
                            .FirstOrDefault())
                        .FirstOrDefault(),
                });

            var queryList = escalatedTicketsQuery.ToList();

            // Apply Sorting
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                string orderByString = $"{sortField} {sortOrder}";
                queryList = queryList.AsQueryable().OrderBy(orderByString).ToList();
            }
            var finalQueryList = queryList.Select(q => new TicketApiModel
            {
                Id = q.Id,
                TicketName = q.TicketName,
                RaisedBy = q.RaisedBy,
                AssignedTo = q.AssignedTo,
                SubmittedDate = q.SubmittedDate.ToString("yyyy-MM-dd hh:mm tt"),
                Priority = q.Priority,
                Status = q.Status,
                Department = q.Department,
                Location = q.Location

            }).ToList();
            // Apply Pagination
            var totalCount = escalatedTicketsQuery.Count();
            finalQueryList = finalQueryList.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            // Return the result in the ManagerTicketResponse format
            return new ManagerTicketResponse<TicketApiModel>
            {
                Data = finalQueryList,
                TotalDataCount = totalCount
            };
        }

        /// <summary>
        /// Retrieves all the tickets raised by employees with their details for L1 Admin.
        /// </summary>
        /// <param name="sortField"></param>
        /// <param name="sortOrder"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        public EmployeeTicketResponse<L1AdminTicketView> GetAllTickets(string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery)
        {
            var query = from t in _context.TBL_TICKET
                        join tp in _context.TBL_PRIORITY on t.PriorityId equals tp.Id
                        join ts in _context.TBL_STATUS on t.StatusId equals ts.Id
                        join tu in _context.TBL_USER on t.UserId equals tu.Id
                        join ue in _context.TBL_EMPLOYEE on tu.EmployeeId equals ue.Id
                        join ued in _context.TBL_EMPLOYEE_DETAIL on ue.Id equals ued.EmployeeId into empDetailJoin
                        from employeeDetail in empDetailJoin.DefaultIfEmpty()
                        join e in _context.TBL_EMPLOYEE on t.AssignedTo equals e.Id into empJoin
                        from employee in empJoin.DefaultIfEmpty()
                        join ul in _context.TBL_LOCATION on employeeDetail.LocationId equals ul.Id into locJoin
                        from loc in locJoin.DefaultIfEmpty()
                        join ud in _context.TBL_DEPARTMENT on employeeDetail.DepartmentId equals ud.Id into deptJoin
                        from dept in deptJoin.DefaultIfEmpty()
                        where string.IsNullOrEmpty(searchQuery) || t.TicketName.Contains(searchQuery)
                        
                        select new L1AdminTicketView
                        {
                           Id = t.Id,
                            TicketName = t.TicketName,
                            RaisedBy = $"{ue.FirstName} {ue.LastName}",
                            Priority = tp.PriorityName,
                            Status = ts.StatusName,
                            Location = loc.LocationName,
                            Department = dept.DeptName,
                            SubmittedDate = t.SubmittedDate.ToString("yyyy-MM-dd"), // Corrected date format
                            AssignedTo = employee != null ? $"{employee.FirstName} {employee.LastName}" : "--------",
                            
                        };

            var result = query.ToList();

            var queryTicketList = query.ToList();

            // Apply Sorting
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                string orderByString = $"{sortField} {sortOrder}";
                queryTicketList = queryTicketList.AsQueryable().OrderBy(orderByString).ToList();
            }

            var queryList = queryTicketList.AsEnumerable().Select(t => new L1AdminTicketView
            {
                Id = t.Id,
                TicketName = t.TicketName,
                RaisedBy = t.RaisedBy,
                Priority = t.Priority,
                Status = t.Status,
                Location = t.Location,
                Department = t.Department,
                SubmittedDate = t.SubmittedDate,
                AssignedTo = t.AssignedTo,
            });

            // Apply Pagination
            var totalCount = query.Count();
            queryList = queryList.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            // Return the results in a paginated response object.
            return new EmployeeTicketResponse<L1AdminTicketView>
            {
                Data = queryList,
                TotalDataCount = totalCount
            };
        }
    }
}