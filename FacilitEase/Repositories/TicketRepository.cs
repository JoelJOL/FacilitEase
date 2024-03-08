using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Repositories
{
    public class TicketRepository : Repository<TBL_TICKET>, ITicketRepository
    {
        /*Removed Repository<TicketApiModel>
        Dont use api model in BearerTokenExtensions inherit in Repository
        use only entity models for generic T*/
        private readonly AppDbContext _context;

        public TicketRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        
    }
}