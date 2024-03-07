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

        public List<TrackingDetailsDto> GetTicketDetails(int ticketId);

        void ForwardTicket(int ticketId, int managerId);

        public void ForwardTicketDeptHead(int ticketId, int employeeId);

        public string GetCommentTextByTicketId(int ticketId);

        public void UpdateCommentTextByTicketId(int ticketId, string newText);

        public void AddComment(TBL_COMMENT comment);

        public Task<bool> DeleteCommentAsync(int ticketId);

        string GetTimeSinceLastUpdate(int ticketId);

        public TicketDetailDataDto GetTicketDetailByAgent(int desiredTicketId);

        public AgentTicketResponse<RaisedTicketsDto> GetTicketsByAgent(int agentId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);

        public AgentTicketResponse<ResolvedTicketDto> GetResolvedTicketsByAgent(int agentId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);

        public AgentTicketResponse<ResolvedTicketDto> GetOnHoldTicketsByAgent(int userId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);

        public AgentTicketResponse<ResolvedTicketDto> GetCancelRequestTicketsByAgent(int agentId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);
    }
}