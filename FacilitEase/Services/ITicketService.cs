using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public interface ITicketService
    {
        IEnumerable<TicketDto> GetTickets();
        void CreateTicket(TicketDto ticketDto);
    }
}
