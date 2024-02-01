using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Services
{
    public interface IL3AdminService
    {
        void AddTicket(TBL_TICKET ticket);
        void CloseTicket(int ticketId);
        void ForwardTicket(int ticketId, int managerId);
        void ForwardTicketToDept(int ticketId, int deptId);
        public string GetCommentTextByTicketId(int ticketId);
        public void UpdateCommentTextByTicketId(int ticketId, string newText);
        public IEnumerable<TicketJoin> GetEscalatedTicketsByAgent(int agentId);
        IEnumerable<Join> GetTicketDetailByAgent(int ticketId);
        public AgentTicketResponse<TicketJoin> GetTicketsByAgent(int agentId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);
        public AgentTicketResponse<TicketResolveJoin> GetResolvedTicketsByAgent(int agentId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);
      
    }
}