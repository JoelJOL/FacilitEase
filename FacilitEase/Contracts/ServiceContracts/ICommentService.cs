using FacilitEase.Models.ApiModels;

namespace FacilitEase.Contracts.ServiceContracts
{
    public interface ICommentService
    {
        IEnumerable<CommentDto> GetCommentsByTicketId(int ticketId);
    }
}