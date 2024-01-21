using FacilitEase.Models.EntityModels;
using FacilitEase.Models.ApiModels;

namespace FacilitEase.Repositories
{
    public interface ITicketRepository : IRepository<TBL_TICKET>
    {
        IEnumerable<ManagerEmployeeTickets> GetApprovalTicket(int managerId);
        IEnumerable<ManagerEmployeeTickets> GetTicketByManager(int managerId);
        ManagerEmployeeTicketDetailed ViewTicketDetails(int ticketId);
    }
}
