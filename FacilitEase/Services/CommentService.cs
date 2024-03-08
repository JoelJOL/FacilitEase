using FacilitEase.Data;
using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public class CommentService : ICommentService
    {
        private readonly AppDbContext _context;

        public CommentService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// To get the comments of a ticket
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public IEnumerable<CommentDto> GetCommentsByTicketId(int ticketId)
        {
            var comments = _context.TBL_COMMENT
                .Where(comment => comment.TicketId == ticketId)
                .Select(comment => new CommentDto
                {
                    Id = comment.Id,
                    TicketId = comment.TicketId,
                    Text = comment.Text,
                })
                .ToList();

            return comments;
        }
    }
}