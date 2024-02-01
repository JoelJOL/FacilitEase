using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public interface IEmployeeService
    {
        List<ManagerSubordinateEmployee> GetSubordinates(int managerId);
        IEnumerable<AgentApiModel> GetAgents(int departmentId);
        IEnumerable<AgentDetailsModel> GetAgentsByDepartment(int departmentId);
        void AddEmployees(IEnumerable<EmployeeInputModel> employeeInputs1, params EmployeeInputModel[] employeeInputs);
        void DeleteEmployee(int id);
        IEnumerable<EmployeeDetails> GetEmployeeDetails(int empId);
    }
}
