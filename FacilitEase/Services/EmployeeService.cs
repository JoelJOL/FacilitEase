using FacilitEase.Data;
using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public class EmployeeService: IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public EmployeeService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;

        }
        public List<ManagerSubordinateEmployee> GetSubordinates(int managerId)
        {
            var result = _context.TBL_EMPLOYEE
                .Where(e => e.ManagerId == managerId)
                .Join(
                    _context.TBL_EMPLOYEE_DETAIL,
                    employee => employee.Id,
                    employeeDetail => employeeDetail.EmployeeId,
                    (employee, employeeDetail) => new ManagerSubordinateEmployee
                    {
                        EmployeeCode = employee.EmployeeCode,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        DOB = employee.DOB,
                        Email = employee.Email,
                        Gender = employee.Gender,
                        Department = _context.TBL_DEPARTMENT.FirstOrDefault(d => d.Id == employeeDetail.DepartmentId).DeptName,
                        Position = _context.TBL_POSITION.FirstOrDefault(p => p.Id == employeeDetail.PositionId).PositionName,
                        Location = _context.TBL_LOCATION.FirstOrDefault(l => l.Id == employeeDetail.LocationId).LocationName

                    })
                .ToList();

            return result;
        }
        public IEnumerable<AgentApiModel> GetAgents(int departmentId)
        {
            var agentRoleId = _context.TBL_USER_ROLE
                .Where(role => role.UserRoleName == "Agent")
                .Select(role => role.Id)
                .FirstOrDefault();

            if (agentRoleId == 0)
            {
                return new List<AgentApiModel>();
            }

            var agents = _context.TBL_USER_ROLE_MAPPING
                .Where(mapping => mapping.UserRoleId == agentRoleId)
                .Join(_context.TBL_USER, mapping => mapping.UserId, user => user.Id, (mapping, user) => new
                {
                    UserId = user.Id,
                    DepartmentId = _context.TBL_EMPLOYEE_DETAIL
                        .Where(detail => detail.EmployeeId == user.EmployeeId)
                        .Select(detail => detail.DepartmentId)
                        .FirstOrDefault()
                })
                .Where(mapping => mapping.DepartmentId == departmentId)
                .Join(_context.TBL_EMPLOYEE, mapping => mapping.UserId, employee => employee.Id, (mapping, employee) => new AgentApiModel
                {
                    AgentId = employee.Id,
                    AgentName = $"{employee.FirstName} {employee.LastName}"
                })
                .ToList();

            return agents;
        }

        public IEnumerable<AgentDetailsModel> GetAgentsByDepartment(int departmentId)
        {
            var agentRoleId = _context.TBL_USER_ROLE
                .Where(role => role.UserRoleName == "Agent")
                .Select(role => role.Id)
                .FirstOrDefault();

            var agentDetails = _context.TBL_USER_ROLE_MAPPING
                .Where(ur => ur.UserRoleId == agentRoleId)
                .Join(_context.TBL_USER, ur => ur.UserId, u => u.Id, (ur, u) => u)
                .Join(_context.TBL_EMPLOYEE_DETAIL, u => u.EmployeeId, ed => ed.EmployeeId, (u, ed) => new
                {
                    EmployeeId = u.EmployeeId,
                    DepartmentId = ed.DepartmentId
                })
                .Where(ed => ed.DepartmentId == departmentId)
                .Join(_context.TBL_EMPLOYEE, e => e.EmployeeId, emp => emp.Id, (e, emp) => new AgentDetailsModel
                {
                    EmployeeCode = emp.EmployeeCode,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    DOB = emp.DOB,
                    Email = emp.Email,
                    Gender = emp.Gender
                })
                .ToList();

            return agentDetails;
        }
    }
}
