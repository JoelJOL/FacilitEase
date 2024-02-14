using FacilitEase.Models.ApiModels;
using Microsoft.AspNetCore.Mvc;

namespace FacilitEase.Services
{
    public interface ITicketService
    {/// <summary>
     /// Changes the status of a ticket.
     /// </summary>
     /// <param name="ticketId">The ID of the ticket to change.</param>
     /// <param name="request">The request containing the new status.</param>
     /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating the success of the operation.</returns>
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
        /// <summary>
        /// retrieve assigned tickets with optional search criteria
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="sortOrder"></param>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        ManagerTicketResponse<TicketApiModel> GetAssignedTickets(int userId, int pageIndex, int pageSize, string sortField, string sortOrder, string searchQuery);
        /// <summary>
        /// retrieve detailed information about a specific ticket
        /// </summary>
        /// <param name="desiredTicketId"></param>
        /// <returns></returns>
        TicketDetails GetTicketDetails(int desiredTicketId);
        /// <summary>
        /// retrieve unassigned tickets with optional search criteria
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="sortOrder"></param>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        ManagerTicketResponse<UnassignedTicketModel> GetUnassignedTickets(int userId, int pageIndex, int pageSize, string sortField, string sortOrder, string searchQuery);
        /// <summary>
        /// Assign the ticket to the agent
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="agentId"></param>
        void AssignTicketToAgent(int userId, int ticketId, int agentId);
        /// <summary>
        /// retrieve escalated tickets with optional search criteria
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="sortOrder"></param>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        ManagerTicketResponse<TicketApiModel> GetEscalatedTickets(int userId, int pageIndex, int pageSize, string sortField, string sortOrder, string searchQuery);

        void CreateTicketWithDocuments([FromForm] TicketDto ticketDto, [FromForm] IFormFile file);

        public void UpdateTicketTracking(int ticketId, int statusId, int? assignedTo, int? controllerId, DateTime? ticketRaisedTimestamp, int updatedBy);

        IEnumerable<DocumentDto> GetDocumentsByTicketId(int ticketId);

        public string GetTimeSinceLastUpdate(int ticketId);

        bool RequestToCancelTicket(int ticketId);

        
    }
}