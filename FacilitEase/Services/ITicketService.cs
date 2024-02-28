using FacilitEase.Models.ApiModels;
using Microsoft.AspNetCore.Mvc;

namespace FacilitEase.Services
{
    public interface ITicketService
    {
        public ManagerTicketResponse<ManagerEmployeeTickets> GetTicketByManager(int managerId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);

        public ManagerTicketResponse<ManagerEmployeeTickets> GetLiveTicketByManager(int managerId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);

        public ManagerEmployeeTicketDetailed ViewTicketDetails(int ticketId);

        public ManagerTicketResponse<ManagerEmployeeTickets> GetApprovalTicket(int managerId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);

        public void TicketDecision(int ticketId, int statusId);

        public void ChangePriority(int ticketId, int newPriorityId);

        public void SendForApproval(int ticketId, int managerId);

        public DepartmentHeadManagerTicketDetails DHTicketDetails(int ticketId);

        public DepartmentHeadTicketResponse<DepartmentHeadManagerTickets> DHGetApprovalTicket(int departmentHeadId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);

        Task<bool> ChangeTicketStatus(int ticketId, TicketStatusChangeRequest request);

        ManagerTicketResponse<TicketApiModel> GetAssignedTickets(int userId, int pageIndex, int pageSize, string sortField, string sortOrder, string searchQuery);

        TicketDetails GetTicketDetails(int desiredTicketId);

        ManagerTicketResponse<UnassignedTicketModel> GetUnassignedTickets(int userId, int pageIndex, int pageSize, string sortField, string sortOrder, string searchQuery);

        void AssignTicketToAgent(int userId, int ticketId, int agentId);

        ManagerTicketResponse<TicketApiModel> GetEscalatedTickets(int userId, int pageIndex, int pageSize, string sortField, string sortOrder, string searchQuery);
        /// <summary> working
        /// To create a new ticket along with associated documents in the database.
        /// </summary>
        /// <param name="ticketDto"></param>
        /// 
        void CreateTicketWithDocuments([FromForm] TicketDto ticketDto, [FromForm] IFormFile file);

        public void UpdateTicketTracking(int ticketId, int statusId, int? assignedTo, int? controllerId, DateTime? ticketRaisedTimestamp, int updatedBy);

        IEnumerable<DocumentDto> GetDocumentsByTicketId(int ticketId);

        public string GetTimeSinceLastUpdate(int ticketId);

        /// <summary>
        /// To cancel a particular ticket
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        bool RequestToCancelTicket(int ticketId);

        
    }
}