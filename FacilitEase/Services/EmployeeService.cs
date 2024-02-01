using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using FacilitEase.Repositories;
using FacilitEase.Services;
using FacilitEase.UnitOfWork;
using System;
using System.Diagnostics;

namespace FacilitEase.Services
{

    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public EmployeeService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public void AddEmployees(IEnumerable<EmployeeInputModel> employeeInputs, params EmployeeInputModel[] additionalEmployeeInputs)
        {
            if (employeeInputs == null || !employeeInputs.Any())
            {
                throw new ArgumentException("Employee input data is null or empty.", nameof(employeeInputs));
            }
            try
            {
                // Combine the two collections if needed
                var allEmployeeInputs = employeeInputs.Concat(additionalEmployeeInputs);

                // Map the input models to your entity models
                var employeeEntities = allEmployeeInputs.Select(employeeInput => new TBL_EMPLOYEE
                {
                    EmployeeCode = employeeInput.EmployeeCode,
                    FirstName = employeeInput.FirstName,
                    LastName = employeeInput.LastName,
                    DOB = employeeInput.DOB,
                    Email = employeeInput.Email,
                    Gender = employeeInput.Gender,
                    ManagerId = employeeInput.ManagerId,
                    // Map other properties as needed
                }).ToList();

                // Add additional business logic if needed before calling the repository
                _unitOfWork.EmployeeRepository.AddRange(employeeEntities);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Log the exception details or print to console for debugging
                Debug.WriteLine($"Error in AddEmployees: {ex.Message}");
                // Log or print the inner exception details
                Debug.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                throw; // Re-throw the exception to propagate it up the call stack
            }
        }
        public void DeleteEmployee(int id)
        {
            try
            {
                var employee = _unitOfWork.EmployeeRepository.GetById(id);

                if (employee == null)
                {
                    throw new KeyNotFoundException($"Employee with ID {id} not found.");
                }

                // Add additional business logic if needed before calling the repository
                _unitOfWork.EmployeeRepository.Delete(employee);
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                // Log the exception details or print to console for debugging
                Console.WriteLine($"Error in DeleteEmployee: {ex.Message}");
                // Log or print the inner exception details
                Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                throw; // Re-throw the exception to propagate it up the call stack
            }
        }

        /// <summary>
        /// retrieve subordinate employees based on the provided managerId
        /// </summary>
        /// <param name="managerId"></param>
        /// <returns></returns>
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
                        EmployeeCode = employee.EmployeeCode.ToString(),
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

        /// <summary>
        /// Retrieve agents based on the RoleId, UserId and DepartmentId
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// retrieve the detailed informations of the agents in a department
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
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
                    EmployeeCode = emp.EmployeeCode.ToString(),
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