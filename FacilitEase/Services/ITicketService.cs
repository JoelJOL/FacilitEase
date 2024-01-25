using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public interface ITicketService
    {
        void CreateTicketWithDocuments(TicketDto ticket);
    }
}
