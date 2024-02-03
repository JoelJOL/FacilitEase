using System.Collections.Generic;
using System.Linq;
using FacilitEase.Models.ApiModels;
using FacilitEase.Data;
using FacilitEase.Services;
using FacilitEase.UnitOfWork;
using System.Net.Sockets;
using Microsoft.Data.SqlClient;
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
        public EmployeeTicketResponse<TicketDetailsDto> GetTicketDetailsByUserId(int userId,  string sortField, string sortOrder, int pageIndex, int pageSize, string  searchQuery)
        {
            var query = from t in _context.TBL_TICKET
                        join ts in _context.TBL_STATUS on t.StatusId equals ts.Id
                        join tp in _context.TBL_PRIORITY on t.PriorityId equals tp.Id
                        join u in _context.TBL_USER on t.AssignedTo equals u.Id
                        join e in _context.TBL_EMPLOYEE on u.Id equals e.Id
                        where t.UserId == userId
                        where string.IsNullOrEmpty(searchQuery) || t.TicketName.Contains(searchQuery)
                        select new TicketDetailsDto
                        {
                            Id = t.Id,
                            TicketName = t.TicketName,
                            TicketDescription = t.TicketDescription,
                            Status = ts.StatusName,
                            AssignedTo = e.FirstName,
                            Priority = tp.PriorityName
                        };

            var queryList = query.ToList();

            // Apply Sorting
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                string orderByString = $"{sortField} {sortOrder}";
                queryList = queryList.AsQueryable().OrderBy(orderByString).ToList();
            }

            // Apply Pagination
            var totalCount = queryList.Count();
            queryList = queryList.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            // Return the results in a paginated response object.
            return new EmployeeTicketResponse<TicketDetailsDto>
            {
                Data = queryList,
                TotalDataCount = totalCount
            };
        }
    
        /// <summary>
        /// To cancel a particular ticket
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public bool RequestToCancelTicket(int ticketId)
        {
            var ticket = _context.TBL_TICKET.FirstOrDefault(t => t.Id == ticketId);

            if (ticket == null)
            {
                return false;
            }

            ticket.StatusId = 6;
            _context.SaveChanges();

            return true;
        }
    }
}
