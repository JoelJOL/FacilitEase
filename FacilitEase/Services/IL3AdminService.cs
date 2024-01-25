using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Services
{
    public interface IL3AdminService
    {
        void AddTicket(TBL_TICKET ticket);
        IEnumerable<TBL_TICKET> GetAllTickets();

        TBL_TICKET GetTicketById(int TicketId);

        void CloseTicket(int ticketId);
        void ForwardTicket(int ticketId, int managerId);
        void ForwardTicketToDept(int ticketId,int deptId);
 
        public IEnumerable<TicketJoin> GetResolvedTicketsByAgent(int agentId);
        IEnumerable<TicketJoin> GetLatestTicketsByAgent(int agentId);
        IEnumerable<Join> GetTicketDetailByAgent(int ticketId);
        public IEnumerable<TicketJoin> GetTicketDetailsByAgent(int agent);
    }
}
