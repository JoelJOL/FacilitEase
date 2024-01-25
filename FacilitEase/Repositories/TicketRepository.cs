using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using System.Collections.Generic;
using System.Linq;

namespace FacilitEase.Repositories
{
    public class TicketRepository : Repository<TicketApiModel>, ITicketRepository
    {
        private readonly AppDbContext _context;
        public TicketRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}

