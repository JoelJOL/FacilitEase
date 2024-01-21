using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using System.Reflection.Metadata;

namespace FacilitEase.Repositories
{
    public class TicketRepository : Repository<TBL_TICKET>, ITicketRepository
    {
        private readonly AppDbContext _context;
        public TicketRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public IEnumerable<ManagerEmployeeTickets> GetApprovalTicket(int managerId)
        {
            var tickets = _context.TBL_TICKET
            .Where(ticket => ticket.ControllerId == managerId)
            .Select(ticket => new ManagerEmployeeTickets
            {
                Id = ticket.Id,
                TicketName = ticket.TicketName,
                EmployeeName = _context.TBL_USER
                    .Where(user => user.Id == ticket.UserId)
                    .Select(user => _context.TBL_EMPLOYEE
                    .Where(employee => employee.Id == user.EmployeeId)
                    .Select(employee => $"{employee.FirstName} {employee.LastName}")
                    .FirstOrDefault())
                .FirstOrDefault(),
                AssignedTo = _context.TBL_EMPLOYEE
                .Where(employee => employee.Id == ticket.AssignedTo)
                .Select(employee => $"{employee.FirstName} {employee.LastName}")
                    .FirstOrDefault(),
                SubmittedDate = ticket.SubmittedDate,
                Priority = _context.TBL_PRIORITY
                .Where(priority => priority.Id == ticket.PriorityId)
                .Select(priority => $"{priority.PriorityName}")
                .FirstOrDefault(),
                Status = _context.TBL_STATUS
                .Where(status => status.Id == ticket.StatusId)
                .Select(status => $"{status.StatusName}")
                .FirstOrDefault(),
            })
            .ToList();

            return tickets;
        }
        public IEnumerable<ManagerEmployeeTickets> GetTicketByManager(int managerId)
        {
            var tickets = _context.TBL_TICKET
        .Where(ticket =>
            _context.TBL_USER
                .Where(user => user.Id == ticket.UserId)
                .Join(
                    _context.TBL_EMPLOYEE,
                    user => user.EmployeeId,
                    employee => employee.Id,
                    (user, employee) => employee.ManagerId == managerId
                )
                .Any()
        )
        .Select(ticket => new ManagerEmployeeTickets
        {
            Id = ticket.Id,
            TicketName = ticket.TicketName,
            EmployeeName = _context.TBL_USER
                    .Where(user => user.Id == ticket.UserId)
                    .Select(user => _context.TBL_EMPLOYEE
                    .Where(employee => employee.Id == user.EmployeeId)
                    .Select(employee => $"{employee.FirstName} {employee.LastName}")
                    .FirstOrDefault())
                .FirstOrDefault(),
            AssignedTo = _context.TBL_EMPLOYEE
                .Where(employee => employee.Id == ticket.AssignedTo)
                .Select(employee => $"{employee.FirstName} {employee.LastName}")
                    .FirstOrDefault(),
            SubmittedDate = ticket.SubmittedDate,
            Priority = _context.TBL_PRIORITY
                .Where(priority => priority.Id == ticket.PriorityId)
                .Select(priority => $"{priority.PriorityName}")
                .FirstOrDefault(),
            Status = _context.TBL_STATUS
                .Where(status => status.Id == ticket.StatusId)
                .Select(status => $"{status.StatusName}")
                .FirstOrDefault(),
        })
            .ToList();

            return tickets;
        }
        public ManagerEmployeeTicketDetailed ViewTicketDetails(int ticketId)
        {
            var ticket = _context.TBL_TICKET
                .Where(t => t.Id == ticketId)
                .Select(t => new ManagerEmployeeTicketDetailed
                {
                    Id = t.Id,
                    TicketName = t.TicketName,
                    EmployeeName = _context.TBL_USER
                        .Where(user => user.Id == t.UserId)
                        .Select(user => _context.TBL_EMPLOYEE
                            .Where(employee => employee.Id == user.EmployeeId)
                            .Select(employee => $"{employee.FirstName} {employee.LastName}")
                            .FirstOrDefault())
                        .FirstOrDefault(),
                    AssignedTo = _context.TBL_EMPLOYEE
                        .Where(employee => employee.Id == t.AssignedTo)
                        .Select(employee => $"{employee.FirstName} {employee.LastName}")
                        .FirstOrDefault(),
                    SubmittedDate = t.SubmittedDate,
                    Priority = _context.TBL_PRIORITY
                        .Where(priority => priority.Id == t.PriorityId)
                        .Select(priority => priority.PriorityName)
                        .FirstOrDefault(),
                    Status = _context.TBL_STATUS
                        .Where(status => status.Id == t.StatusId)
                        .Select(status => status.StatusName)
                        .FirstOrDefault(),
                    TicketDescription = t.TicketDescription,
                    DocumentLink = string.Join(", ", _context.TBL_DOCUMENT
                        .Where(documents => documents.TicketId == t.Id)
                        .Select(document => document.DocumentLink)
                        .ToList())
                })
                .FirstOrDefault();

            return ticket;
        }



    }
}
