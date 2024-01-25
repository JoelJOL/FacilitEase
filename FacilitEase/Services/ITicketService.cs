using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public interface ITicketService
    {
        List<TicketApiModel> GetTickets();
        List<TicketApiModel> GetUnassignedTickets();
        void AssignTicketToAgent(int ticketId, int agentId);
        List<TicketApiModel> GetAssignedTickets();
        List<TicketApiModel> GetEscalatedTickets();
    }
}
