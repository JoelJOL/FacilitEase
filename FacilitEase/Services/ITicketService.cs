using FacilitEase.Models.ApiModels;
using Microsoft.AspNetCore.Mvc;

namespace FacilitEase.Services
{
    public interface ITicketService
    {
        //Abhijith
        public ManagerTicketResponse<ManagerEmployeeTickets> GetTicketByManager(int managerId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);

        //Abhijith
        public ManagerTicketResponse<ManagerEmployeeTickets> GetLiveTicketByManager(int managerId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);

        //Abhijith
        public ManagerEmployeeTicketDetailed ViewTicketDetails(int ticketId);

        //Abhijith
        public ManagerTicketResponse<ManagerEmployeeTickets> GetApprovalTicket(int managerId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);

        //Abhijith
        public void TicketDecision(int ticketId, int statusId);

        //Abhijith
        public void ChangePriority(int ticketId, int newPriorityId);

        //Abhijith
        public void SendForApproval(int ticketId, int managerId);

        //Avinash
        public DepartmentHeadManagerTicketDetails DHTicketDetails(int ticketId);

        //Abhijith
        public DepartmentHeadTicketResponse<DepartmentHeadManagerTickets> DHGetApprovalTicket(int departmentHeadId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);

        Task<bool> ChangeTicketStatus(int ticketId, TicketStatusChangeRequest request);

        ManagerTicketResponse<TicketApiModel> GetAssignedTickets(int userId, int pageIndex, int pageSize, string sortField, string sortOrder, string searchQuery);

        //Nathaniel
        TicketDetails GetTicketDetails(int desiredTicketId);

        //Nathaniel
        ManagerTicketResponse<UnassignedTicketModel> GetUnassignedTickets(int userId, int pageIndex, int pageSize, string sortField, string sortOrder, string searchQuery);

        //Nathaniel
        void AssignTicketToAgent(int userId, int ticketId, int agentId);
      
        //Nathaniel
        ManagerTicketResponse<TicketApiModel> GetEscalatedTickets(int userId, int pageIndex, int pageSize, string sortField, string sortOrder, string searchQuery);
        //Hema
        //void CreateTicketWithDocuments(TicketDto ticket);
        void CreateTicketWithDocuments([FromForm] TicketDto ticketDto, [FromForm] IFormFile file);
        //Hema
        //Julia
        public void UpdateTicketTracking(int ticketId, int statusId, int? assignedTo, int? controllerId, DateTime? ticketRaisedTimestamp, int updatedBy);

        IEnumerable<DocumentDto> GetDocumentsByTicketId(int ticketId);
        public string GetTimeSinceLastUpdate(int ticketId);

        bool RequestToCancelTicket(int ticketId);
    }
}