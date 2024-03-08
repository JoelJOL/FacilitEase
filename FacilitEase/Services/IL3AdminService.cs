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

        public List<RetrieveCommentDto> GetCommentTextsByTicketId(int ticketId);

        public RetrieveCommentDto UpdateComment(int commentId, string text);

        public RetrieveCommentDto AddComment(CommentRequestDto commentRequestDto);

        public void DeleteComment(int commentId);

        string GetTimeSinceLastUpdate(int ticketId);

        public TicketDetailDataDto GetTicketDetailByAgent(int desiredTicketId);

        public AgentTicketResponse<RaisedTicketsDto> GetTicketsByAgent(int agentId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);

        public AgentTicketResponse<ResolvedTicketDto> GetResolvedTicketsByAgent(int agentId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);

        public AgentTicketResponse<ResolvedTicketDto> GetOnHoldTicketsByAgent(int userId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);

        public AgentTicketResponse<ResolvedTicketDto> GetCancelRequestTicketsByAgent(int agentId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery);
    }
}