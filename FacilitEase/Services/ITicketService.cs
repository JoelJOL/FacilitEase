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

        /// <summary>
        /// Get - Retrieves a list of tickets raised by the emloyees that are currently live,  working under a specific manager and the total number of tickets
        /// Includes pagination,searching and sorting functionality.
        /// </summary>
        /// <param name="managerId"></param>
        /// <param name="sortField"></param>
        /// <param name="sortOrder"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchQuery"></param>
        /// <returns>A response of paginated list of tickets of employees associated with a specific manager and the total tickets count</returns>
        public ManagerTicketResponse<ManagerEmployeeTickets> GetLiveTicketByManager(int managerId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);

        /// <summary>
        /// Get - Retrieves all the data required when the manager accesses the detailed view of a specific employee ticket
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public ManagerEmployeeTicketDetailed ViewTicketDetails(int ticketId);

        /// <summary>
        /// Get - Retrieves a list of tickets raised by the emloyees that need approval from the manager and the total number of waiting tickets
        /// Includes pagination,searching and sorting functionality.
        /// </summary>
        /// <param name="managerId"></param>
        /// <param name="sortField"></param>
        /// <param name="sortOrder"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        public ManagerTicketResponse<ManagerEmployeeTickets> GetApprovalTicket(int managerId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);

        /// <summary>
        /// Post - Decides whether the ticket needs to be accepted or rejected
        /// Updates the status of a ticket to inprogress or cancelled and changes controller to the agent or null.
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="statusId"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void TicketDecision(int ticketId, int statusId);

        /// <summary>
        /// Post - Updates the priority id according to the priority specified by the Manager
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="newPriorityId"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void ChangePriority(int ticketId, int newPriorityId);

        /// <summary>
        /// Post - Updates the controller id to the department head id if the manager things higher authority approval is necessary
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="managerId"></param>
        /// <exception cref="InvalidOperationException"></exception>
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

        /// <summary>
        /// Method to add field to to ticket tracking table
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="categoryId"></param>
        public void UpdateTicketTracking(int ticketId, int statusId, int? assignedTo, int? controllerId, DateTime? ticketRaisedTimestamp, int updatedBy);

        IEnumerable<DocumentDto> GetDocumentsByTicketId(int ticketId);

        /// <summary>
        /// This method gets the time since the comment of the ticket has been last updated.
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="categoryId"></param>
        public string GetTimeSinceLastUpdate(int ticketId);

        /// <summary>
        /// To cancel a particular ticket
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        bool RequestToCancelTicket(int ticketId);
    }
}