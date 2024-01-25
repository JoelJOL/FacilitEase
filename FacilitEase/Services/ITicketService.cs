using System.Threading.Tasks;
﻿using FacilitEase.Models.EntityModels;
using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public interface ITicketService
    {
        void CreateTicketWithDocuments(TicketDto ticket);
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
      Task<bool> ChangeTicketStatus(int ticketId, TicketStatusChangeRequest request);
    }
}
