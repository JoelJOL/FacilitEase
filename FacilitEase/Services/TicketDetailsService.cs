using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using System.Linq.Dynamic.Core;

namespace FacilitEase.Services
{
    public class TicketDetailsService : ITicketDetailsService
    {
        private readonly AppDbContext _context;

        public TicketDetailsService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// To get all the tickets raised by an employee
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sortField"></param>
        /// <param name="sortOrder"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        public EmployeeTicketResponse<TicketDetailsDto> GetTicketDetailsByUserId(int userId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery)
        {
            var query = from t in _context.TBL_TICKET
                        join ts in _context.TBL_STATUS on t.StatusId equals ts.Id
                        join tp in _context.TBL_PRIORITY on t.PriorityId equals tp.Id
                        join u in _context.TBL_USER on t.AssignedTo equals u.Id into userJoin
                        from u in userJoin.DefaultIfEmpty()  // Left Join
                        join e in _context.TBL_EMPLOYEE on u.Id equals e.Id into employeeJoin
                        from e in employeeJoin.DefaultIfEmpty()  // Left Join
                        where t.UserId == userId
                        where string.IsNullOrEmpty(searchQuery) || t.TicketName.Contains(searchQuery)
                        select new TicketDetailsDto
                        {
                            Id = t.Id,
                            TicketName = t.TicketName,
                            Status = ts.StatusName,
                            AssignedTo = e != null ? e.FirstName : null,  // Check for null to handle left join
                            Priority = tp.PriorityName
                        };

            // Apply Sorting
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                string orderByString = $"{sortField} {sortOrder}";
                query = query.OrderBy(orderByString);
            }

            // Apply Pagination
            var totalCount = query.Count();
            var paginatedQuery = query.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            // Return the results in a paginated response object.
            return new EmployeeTicketResponse<TicketDetailsDto>
            {
                Data = paginatedQuery,
                TotalDataCount = totalCount
            };
        }

        public TicketDetailsDto GetTicketDetailsById(int ticketId)
        {
            var query = from t in _context.TBL_TICKET
                        join ts in _context.TBL_STATUS on t.StatusId equals ts.Id
                        join tp in _context.TBL_PRIORITY on t.PriorityId equals tp.Id
                        join u in _context.TBL_USER on t.AssignedTo equals u.Id into userJoin
                        from u in userJoin.DefaultIfEmpty()
                        join e in _context.TBL_EMPLOYEE on u.Id equals e.Id into employeeJoin
                        from e in employeeJoin.DefaultIfEmpty()
                        where t.Id == ticketId
                        select new TicketDetailsDto
                        {
                            Id = t.Id,
                            TicketName = t.TicketName,
                            Status = ts.StatusName,
                            AssignedTo = e != null ? e.FirstName : null,
                            Priority = tp.PriorityName,
                            SubmissionDate = t.SubmittedDate 
                        };

            return query.FirstOrDefault();
        }

        /// <summary>
        /// To cancel a particular ticket
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public bool RequestToCancelTicket(int ticketId)
        {
            var ticket = _context.TBL_TICKET.FirstOrDefault(t => t.Id == ticketId);

            if (ticket == null || !IsValidTicketStatus(ticket.StatusId))
            {
                return false;
            }

            ticket.StatusId = 7;
            ticket.ControllerId = ticket.AssignedTo; 
            _context.SaveChanges();

            return true;
        }

        private bool IsValidTicketStatus(int? statusId)
        {
            // Check if the status is open or in progress (1, 2, 3)
            return statusId.HasValue && (statusId == 1 || statusId == 2 || statusId == 3 || statusId == 6);
        }

    }
}