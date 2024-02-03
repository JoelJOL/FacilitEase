using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public interface ITicketDetailsService
    {
        bool RequestToCancelTicket(int ticketId);

        EmployeeTicketResponse<TicketDetailsDto> GetTicketDetailsByUserId(int userId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);
    }
}