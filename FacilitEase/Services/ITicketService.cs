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

        /// <summary>
        /// Retrieves the details of a ticket for a department head or manager.
        /// </summary>
        /// <param name="ticketId">The ID of the ticket to retrieve.</param>
        /// <returns>A DepartmentHeadManagerTicketDetails object containing the detailed ticket view on selecting a particular ticket</returns>

        public DepartmentHeadManagerTicketDetails DHTicketDetails(int ticketId);

        /// <summary>
        /// Retrieves a paginated list of tickets for approval by a department head.
        /// </summary>
        /// <param name="departmentHeadId">The ID of the department head.</param>
        /// <param name="sortField">The field to sort the tickets by.</param>
        /// <param name="sortOrder">The order to sort the tickets in.</param>
        /// <param name="pageIndex">The index of the page to retrieve.</param>
        /// <param name="pageSize">The size of the page to retrieve.</param>
        /// <param name="searchQuery">The query to filter the tickets by.</param>
        /// <returns>A DepartmentHeadTicketResponse object containing the paginated list of tickets and the total count of tickets.</returns>

        public DepartmentHeadTicketResponse<DepartmentHeadManagerTickets> DHGetApprovalTicket(int departmentHeadId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);

        /// <summary>
        /// Changes the status of a ticket.
        /// </summary>
        /// <param name="ticketId">The ID of the ticket to change.</param>
        /// <param name="request">The request containing the new status.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating the success of the operation.</returns>
        Task<bool> ChangeTicketStatus(int ticketId, TicketStatusChangeRequest request);

        ManagerTicketResponse<TicketApiModel> GetAssignedTickets(int userId, int pageIndex, int pageSize, string sortField, string sortOrder, string searchQuery);

        TicketDetails GetTicketDetails(int desiredTicketId);

        ManagerTicketResponse<UnassignedTicketModel> GetUnassignedTickets(int userId, int pageIndex, int pageSize, string sortField, string sortOrder, string searchQuery);

        void AssignTicketToAgent(int userId, int ticketId, int agentId);

        ManagerTicketResponse<TicketApiModel> GetEscalatedTickets(int userId, int pageIndex, int pageSize, string sortField, string sortOrder, string searchQuery);

        void CreateTicketWithDocuments([FromForm] TicketDto ticketDto, [FromForm] IFormFile file);

        public void UpdateTicketTracking(int ticketId, int statusId, int? assignedTo, int? controllerId, DateTime? ticketRaisedTimestamp, int updatedBy);

        IEnumerable<DocumentDto> GetDocumentsByTicketId(int ticketId);

        public string GetTimeSinceLastUpdate(int ticketId);

        bool RequestToCancelTicket(int ticketId);

        
    }
}