using System.Collections.Generic;
using System.Linq;
using FacilitEase.Models.ApiModels;
using FacilitEase.Data;
using FacilitEase.Services;
using FacilitEase.UnitOfWork;

namespace FacilitEase.Services
{
    public class TicketDetailsService : ITicketDetailsService
    {

        private readonly AppDbContext _context;

        public TicketDetailsService(AppDbContext context)
        {

            _context = context;
        }

        public IEnumerable<TicketDetailsDto> GetTicketDetailsByUserId(int userId)
        {
            var query = from t in _context.TBL_TICKET
                        join ts in _context.TBL_STATUS on t.StatusId equals ts.Id
                        join tp in _context.TBL_PRIORITY on t.PriorityId equals tp.Id
                        join u in _context.TBL_USER on t.AssignedTo equals u.Id
                        where t.UserId == userId
                        select new TicketDetailsDto
                        {
                            Id = t.Id,
                            TicketName = t.TicketName,
                            TicketDescription = t.TicketDescription,
                            StatusId = ts.StatusName,
                            AssignedTo = u.Email,
                            PriorityId = tp.PriorityName
                        };

            return query.ToList();
        }

    }
}
