using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Contracts.ServiceContracts
{
    public interface IEmployeeService
    {
        List<ManagerSubordinateEmployee> GetSubordinates(int managerId);

        IEnumerable<AgentApiModel> GetAgents(int userId);

        IEnumerable<AgentDetailsModel> GetAgentsByDepartment(int userId);

        public IEnumerable<TBL_LOCATION> GetLocations();

        public IEnumerable<TBL_POSITION> GetPositions();

        IEnumerable<EmployeeDetails> GetEmployeeDetails(int empId);

        List<ProjectEmployeeDetails> GetEmployeesByProject(int userId);
    }
}