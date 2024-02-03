using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public interface ICommentService
    {
        IEnumerable<CommentDto> GetCommentsByTicketId(int ticketId);
    }
}
