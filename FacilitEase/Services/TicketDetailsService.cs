using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using System.Linq;
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
                            join u in _context.TBL_USER on t.AssignedTo equals u.Id
                            join e in _context.TBL_EMPLOYEE on u.Id equals e.Id
                            where t.UserId == userId
                            where string.IsNullOrEmpty(searchQuery) || t.TicketName.Contains(searchQuery)
                            select new
                            {
                                Id = t.Id,
                                TicketName = t.TicketName,
                                SubmittedDate = t.SubmittedDate,
                                AssignedTo = e.FirstName,
                                Priority = tp.PriorityName,
                                Status = ts.StatusName,
                               
                            };
            var queryTicketList = query.ToList(); 
                // Apply Sorting
                if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
                {
                    string orderByString = $"{sortField} {sortOrder}";
                queryTicketList = queryTicketList.AsQueryable().OrderBy(orderByString).ToList();
                }

                var queryList = queryTicketList.AsEnumerable().Select(t => new TicketDetailsDto
                {
                    Id = t.Id,
                    TicketName = t.TicketName,
                    Status = t.Status,
                    AssignedTo = t.AssignedTo,
                    Priority = t.Priority,
                    SubmittedDate = t.SubmittedDate.ToString("yyyy-MM-dd hh:mm tt"),
                });

                // Apply Pagination
                var totalCount = queryList.Count();
                var paginatedQuery = queryList.Skip(pageIndex * pageSize).Take(pageSize).ToList();

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