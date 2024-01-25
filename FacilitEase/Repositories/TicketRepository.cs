using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using System.Reflection.Metadata;
using System.Linq.Dynamic.Core;


namespace FacilitEase.Repositories
{
    public class TicketRepository : Repository<TBL_TICKET>, ITicketRepository
    {
        private readonly AppDbContext _context;
        public TicketRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        


    }
}
