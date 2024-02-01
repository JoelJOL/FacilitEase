using System.Threading.Tasks;
﻿using FacilitEase.Models.EntityModels;
using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public interface ITicketService
    {
        //Abhijith
        public ManagerTicketResponse<ManagerEmployeeTickets> GetTicketByManager(int managerId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);
        //Abhijith
        public ManagerEmployeeTicketDetailed ViewTicketDetails(int ticketId);
        //Abhijith
        public ManagerTicketResponse<ManagerEmployeeTickets> GetApprovalTicket(int managerId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);
        //Abhijith
        public void TicketDecision(int ticketId, int statusId);
        //Abhijith
        public void ChangePriority(int ticketId, int newPriorityId);
        //Abhijith
        public void SendForApproval(int ticketId , int managerId);
        //Avinash
        Task<bool> ChangeTicketStatus(int ticketId, TicketStatusChangeRequest request);
        //Nathaniel
        List<TicketApiModel> GetTickets();
        //Nathaniel
        List<TicketApiModel> GetUnassignedTickets();
        //Nathaniel
        void AssignTicketToAgent(int ticketId, int agentId);
        //Nathaniel
        List<TicketApiModel> GetAssignedTickets();
        //Nathaniel
        List<TicketApiModel> GetEscalatedTickets();
        //Hema
        void CreateTicketWithDocuments(TicketDto ticket);
    }
}
