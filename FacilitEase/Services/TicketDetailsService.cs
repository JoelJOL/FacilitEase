using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using System.Linq.Dynamic.Core;

namespace FacilitEase.Services
{
    public class TicketDetailsService : ITicketDetailsService
    {
        private readonly AppDbContext _context;
        private readonly TicketService _ticketService;

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
                            Priority = tp.PriorityName,
                            SubmittedDate = t.SubmittedDate.ToString("dd-MM-yy hh:mm:tt"),
                        };

            // Apply Sorting
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                string orderByString = $"{sortField} {sortOrder}";
                query = query.OrderBy(orderByString);
            }
            var queryList = query.ToList();
            // Apply Pagination
            var totalCount = queryList.Count();
            var paginatedQuery = queryList.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            // Return the results in a paginated response obgit barncject.
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
                        };

            return query.FirstOrDefault();
        }

        /// <summary>
        /// To cancel a particular ticket
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
    }
}