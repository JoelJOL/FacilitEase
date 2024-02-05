using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Services
{
    public interface IL3AdminService
    {
        void AddTicket(TBL_TICKET ticket);
        void CloseTicket(int ticketId);
        public void AcceptTicketCancellation(int ticketId);
        public void DenyTicketCancellation(int ticketId);

        void ForwardTicket(int ticketId, int managerId);
        void ForwardTicketToDept(int ticketId, int deptId);
        public string GetCommentTextByTicketId(int ticketId);
        public void UpdateCommentTextByTicketId(int ticketId, string newText);
        public void AddComment(TBL_COMMENT comment);
        public Task<bool> DeleteCommentAsync(int ticketId);
        IEnumerable<Join> GetTicketDetailByAgent(int ticketId);
        public AgentTicketResponse<TicketJoin> GetTicketsByAgent(int agentId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);
        public AgentTicketResponse<TicketResolveJoin> GetResolvedTicketsByAgent(int agentId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);
        public AgentTicketResponse<TicketResolveJoin> GetOnHoldTicketsByAgent(int agentId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);
        public AgentTicketResponse<TicketResolveJoin> GetCancelRequestTicketsByAgent(int agentId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);


    }
}