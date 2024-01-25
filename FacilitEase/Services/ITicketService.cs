using FacilitEase.Models.EntityModels;
using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public interface ITicketService
    {
        public IEnumerable<ManagerEmployeeTickets> GetTicketByManager(int managerId, string sortField, string sortOrder, int pageIndex, int pageSize);
        public ManagerEmployeeTicketDetailed ViewTicketDetails(int ticketId);
        public IEnumerable<ManagerEmployeeTickets> GetApprovalTicket(int employeeId);
        public void TicketDecision(int ticketId, int statusId);
        public void ChangePriority(int ticketId, int newPriorityId);
        public void SendForApproval(int ticketId , int managerId);
        List<TicketApiModel> GetTickets();
        List<TicketApiModel> GetUnassignedTickets();
        void AssignTicketToAgent(int ticketId, int agentId);
        List<TicketApiModel> GetAssignedTickets();
        List<TicketApiModel> GetEscalatedTickets();
    }
}
