using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public interface IEmployeeService
    {
        List<ManagerSubordinateEmployee> GetSubordinates(int managerId);
        IEnumerable<AgentApiModel> GetAgents(int departmentId);
        IEnumerable<AgentDetailsModel> GetAgentsByDepartment(int departmentId);
    }
}
