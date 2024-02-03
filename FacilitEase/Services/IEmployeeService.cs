using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Services
{
    public interface IEmployeeService
    {
        List<ManagerSubordinateEmployee> GetSubordinates(int managerId);        
        IEnumerable<AgentApiModel> GetAgents(int departmentId);
        IEnumerable<AgentDetailsModel> GetAgentsByDepartment(int departmentId);        
        void AddEmployees(IEnumerable<EmployeeInputModel> employeeInputs, params EmployeeInputModel[] additionalEmployeeInputs);
        void DeleteEmployee(int id);
        public  IEnumerable<TBL_LOCATION> GetLocations();
        public IEnumerable<TBL_POSITION> GetPositions();
        IEnumerable<EmployeeDetails> GetEmployeeDetails(int empId);
    }
}
