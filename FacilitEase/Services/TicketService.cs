using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.UnitOfWork;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace FacilitEase.Services
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public TicketService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public void ChangePriority(int ticketId, int newPriorityId)
        {
            var ticket = _unitOfWork.Tickets.GetById(ticketId);
            if (ticket == null)
            {
                throw new InvalidOperationException($"Ticket with ID {ticketId} not found.");
            }
            else
            {
                ticket.PriorityId = newPriorityId;
            }
            _unitOfWork.Complete();
        }


        public void SendForApproval(int ticketId, int managerId)
        {
            var ticket = _unitOfWork.Tickets.GetById(ticketId);
            if (ticket == null)
            {
                throw new InvalidOperationException($"Ticket with ID {ticketId} not found.");
            } 
            else
            {
                var manager = _unitOfWork.Employees.GetById(managerId);
                if (manager?.ManagerId != null)
                {
                    ticket.ControllerId = manager.ManagerId;
                }
                else
                {
                    throw new InvalidOperationException("ManagerId is null or invalid.");
                }
            }
            _unitOfWork.Complete();
            

        }

        public void TicketDecision(int ticketId, int statusId)
        {
            var ticket = _unitOfWork.Tickets.GetById(ticketId);
            if (ticket == null)
            {
                throw new InvalidOperationException($"Ticket with ID {ticketId} not found.");
            }
            else 
            { 
                ticket.StatusId = statusId;
            }
            _unitOfWork.Complete();


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

        public IEnumerable<ManagerEmployeeTickets> GetTicketByManager(int managerId, string sortField, string sortOrder, int pageIndex, int pageSize)
        {
            var query = _context.TBL_TICKET
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
                });

            // Apply Sorting
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder.ToLower())
                {
                    case "asc":
                        query = query.OrderBy(sortField);
                        break;
                    case "desc":
                        query = query.OrderBy($"{sortField} descending");
                        break;
                    default:
                        // Handle invalid sortOrder if needed
                        break;
                }
            }

            // Apply Pagination
            query = query.Skip(pageIndex * pageSize).Take(pageSize);

            var tickets = query.ToList();

            return tickets;
        }

        public IEnumerable<ManagerEmployeeTickets> GetTicketByManager2(int managerId)
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
