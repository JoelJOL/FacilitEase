/*using System.Collections.Generic;
using System.Linq;
using FacilitEase.Models.ApiModels;
using FacilitEase.Data;
using FacilitEase.Services;
using FacilitEase.UnitOfWork;

namespace FacilitEase.Services
{
    public class TicketDetailsService : ITicketDetailsService
    {
        private readonly IUnitOfWork _dbContext;

        public TicketDetailsService(IUnitOfwork dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<TicketDetailsDto> GetTicketDetailsByUserId(int userId)
        {
            var query = from t in _dbContext.Ticket
                        join ts in _dbContext.Status on t.StatusId equals ts.StatusId
                        join tp in _dbContext.Priority on t.PriorityId equals tp.PriorityId
                        join u in _dbContext.User on t.AssignedToUserId equals u.UserId
                        where t.UserId == userId
                        select new TicketDetailsDto
                        {
                            TicketName = t.TicketName,
                            TicketDescription = t.TicketDescription,
                            StatusId = ts.StatusName,
                            AssignedTo = u.UserName,
                            PriorityId = tp.PriorityName
                        };

            return query.ToList();
        }

    }
}
*/