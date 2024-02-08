using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public interface ITicketDetailsService
    {
        TicketDetailsDto GetTicketDetailsById(int ticketId);
        

        EmployeeTicketResponse<TicketDetailsDto> GetTicketDetailsByUserId(int userId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);
    }
}