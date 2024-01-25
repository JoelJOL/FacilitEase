using FacilitEase.Data;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Repositories
{
    public class TicketRepository : Repository<TBL_TICKET>, ITicketRepository
    {
        public TicketRepository(AppDbContext context) : base(context) { }
    }
}

