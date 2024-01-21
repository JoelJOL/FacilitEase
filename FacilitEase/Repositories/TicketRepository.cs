using FacilitEase.Data;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Repositories
{
    public class TicketRepository : Repository<TBL_TICKET>, ITicketRepository
    {
        public TicketRepository(AppDbContext context) : base(context)
        {
        }

        // Implement specific methods related to tickets if needed
    }
}
