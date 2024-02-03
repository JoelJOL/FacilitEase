using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Services
{
    public interface IEmployeeService
    {
        List<ManagerSubordinateEmployee> GetSubordinates(int managerId);        
        IEnumerable<AgentApiModel> GetAgents(int departmentId);
        IEnumerable<AgentDetailsModel> GetAgentsByDepartment(int departmentId);        
        void AddEmployees(IEnumerable<EmployeeInputModel> employeeInputs1, params EmployeeInputModel[] employeeInputs);
        void DeleteEmployee(int id);
        public  IEnumerable<Location> GetLocations();
        public IEnumerable<Position> GetPositions();
        IEnumerable<EmployeeDetails> GetEmployeeDetails(int empId);
    }
}
