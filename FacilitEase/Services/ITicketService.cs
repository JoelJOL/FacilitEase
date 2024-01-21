// ITicketService.cs
using FacilitEase.Models.ApiModels;
using System.Threading.Tasks;

public interface ITicketService
{
    Task<bool> ChangeTicketStatus(int ticketId, TicketStatusChangeRequest request);
    // Other methods...
}
