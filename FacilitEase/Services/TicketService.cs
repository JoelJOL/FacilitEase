using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.Repositories;
using System.Collections.Generic;
using System.Linq;

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

        public List<TicketApiModel> GetTickets()
        {
            var tickets = _context.TBL_TICKET
                .Select(ticket => new TicketApiModel
                {
                    TicketId = ticket.Id,
                    TicketName = ticket.TicketName,
                    TicketDescription = ticket.TicketDescription,
                    RaisedBy = _context.TBL_EMPLOYEE
                        .Where(employee => employee.Id == _context.TBL_USER
                            .Where(user => user.Id == ticket.UserId)
                            .Select(user => user.EmployeeId)
                            .FirstOrDefault())
                        .Select(employee => $"{employee.FirstName} {employee.LastName}")
                        .FirstOrDefault(),
                    AssignedTo = _context.TBL_EMPLOYEE
                        .Where(employee => employee.Id == ticket.AssignedTo)
                        .Select(employee => $"{employee.FirstName} {employee.LastName}")
                        .FirstOrDefault(),
                    RaisedDateTime = ticket.SubmittedDate,
                    Priority = _context.TBL_PRIORITY
                        .Where(priority => priority.Id == ticket.PriorityId)
                        .Select(priority => priority.PriorityName)
                        .FirstOrDefault(),
                    Status = _context.TBL_STATUS
                        .Where(status => status.Id == ticket.StatusId)
                        .Select(status => status.StatusName)
                        .FirstOrDefault()
                    // Add other properties as needed
                })
                .ToList();

            return tickets;
        }

        public List<TicketApiModel> GetUnassignedTickets()
        {
            var unassignedTickets = _context.TBL_TICKET
                .Where(ticket => ticket.AssignedTo == null)
                .Select(ticket => new TicketApiModel
                {
                    TicketId = ticket.Id,
                    TicketName = ticket.TicketName,
                    TicketDescription = ticket.TicketDescription,
                    RaisedBy = _context.TBL_EMPLOYEE
                        .Where(employee => employee.Id == _context.TBL_USER
                            .Where(user => user.Id == ticket.UserId)
                            .Select(user => user.EmployeeId)
                            .FirstOrDefault())
                        .Select(employee => $"{employee.FirstName} {employee.LastName}")
                        .FirstOrDefault(),
                    RaisedDateTime = ticket.SubmittedDate,
                    Priority = _context.TBL_PRIORITY
                        .Where(priority => priority.Id == ticket.PriorityId)
                        .Select(priority => priority.PriorityName)
                        .FirstOrDefault(),
                    Status = _context.TBL_STATUS
                        .Where(status => status.Id == ticket.StatusId)
                        .Select(status => status.StatusName)
                        .FirstOrDefault()
                    // Add other properties as needed
                })
                .ToList();

            return unassignedTickets;
        }

        public void AssignTicketToAgent(int ticketId, int agentId)
        {
            // Find the agent by ID
            var agent = _context.TBL_EMPLOYEE.Find(agentId);

            if (agent != null)
            {
                // Find the ticket by ID
                var ticket = _context.TBL_TICKET.Find(ticketId);

                if (ticket != null)
                {
                    // Assign the ticket to the agent
                    ticket.AssignedTo = agent.Id;
                    ticket.UpdatedDate = DateTime.Now;

                    // Update the status to "In Progress"
                    ticket.StatusId = 2;

                    _context.SaveChanges();
                }
                else
                {
                    // Handle the case where the ticket with the specified ID is not found
                    // You can throw an exception, log an error, or return an error response
                    throw new InvalidOperationException("Ticket not found.");
                    // or log an error: Log.LogError("Ticket not found for ID: " + ticketId);
                    // or return an error response: return BadRequest("Ticket not found.");
                }
            }
            else
            {
                // Handle the case where the agent with the specified ID is not found
                // You can throw an exception, log an error, or return an error response
                throw new InvalidOperationException("Agent not found.");
                // or log an error: Log.LogError("Agent not found with ID: " + agentId);
                // or return an error response: return BadRequest("Agent not found.");
            }
        }
        public List<TicketApiModel> GetAssignedTickets()
        {
            var assignedTickets = _context.TBL_TICKET
                .Where(ticket => ticket.AssignedTo != null)
                .Select(ticket => new TicketApiModel
                {
                    TicketId = ticket.Id,
                    TicketName = ticket.TicketName,
                    TicketDescription = ticket.TicketDescription,
                    AssignedTo = _context.TBL_EMPLOYEE
                        .Where(employee => employee.Id == ticket.AssignedTo)
                        .Select(employee => $"{employee.FirstName} {employee.LastName}")
                        .FirstOrDefault(),
                    RaisedBy = _context.TBL_EMPLOYEE
                        .Where(employee => employee.Id == _context.TBL_USER
                            .Where(user => user.Id == ticket.UserId)
                            .Select(user => user.EmployeeId)
                            .FirstOrDefault())
                        .Select(employee => $"{employee.FirstName} {employee.LastName}")
                        .FirstOrDefault(),
                    RaisedDateTime = ticket.SubmittedDate,
                    Priority = _context.TBL_PRIORITY
                        .Where(priority => priority.Id == ticket.PriorityId)
                        .Select(priority => priority.PriorityName)
                        .FirstOrDefault(),
                    Status = _context.TBL_STATUS
                        .Where(status => status.Id == ticket.StatusId)
                        .Select(status => status.StatusName)
                        .FirstOrDefault()
                    // Add other properties as needed
                })
                .ToList();

            return assignedTickets;
        }
        public List<TicketApiModel> GetEscalatedTickets()
        {
            var escalatedTickets = _context.TBL_TICKET
                .Join(_context.TBL_USER,
                      ticket => ticket.UserId,
                      user => user.Id,
                      (ticket, user) => new { Ticket = ticket, User = user })
                .Join(_context.TBL_EMPLOYEE,
                      joined => joined.User.EmployeeId,
                      employee => employee.Id,
                      (joined, employee) => new TicketApiModel
                      {
                          // Map properties from TBL_TICKET, TBL_USER, and TBL_EMPLOYEE
                          TicketId = joined.Ticket.Id,
                          TicketName = joined.Ticket.TicketName,
                          TicketDescription = joined.Ticket.TicketDescription,
                          RaisedBy = $"{employee.FirstName} {employee.LastName}",
                          Priority = _context.TBL_PRIORITY
                                  .FirstOrDefault(p => p.Id == joined.Ticket.PriorityId) != null ?
                                  _context.TBL_PRIORITY.FirstOrDefault(p => p.Id == joined.Ticket.PriorityId).PriorityName : null,
                          Status = _context.TBL_STATUS
                                .FirstOrDefault(s => s.Id == joined.Ticket.StatusId) != null ?
                                _context.TBL_STATUS.FirstOrDefault(s => s.Id == joined.Ticket.StatusId).StatusName : null,
                          AssignedTo = _context.TBL_EMPLOYEE
                                       .Where(e => e.Id == joined.Ticket.AssignedTo)
                                       .Select(e => $"{e.FirstName} {e.LastName}")
                                       .FirstOrDefault(),
                          RaisedDateTime = joined.Ticket.SubmittedDate,
                          // ... add more properties here
                      })
                .Where(ticket => ticket.Status == "Escalated") // Filter by "Escalated" status
                .ToList();

            return escalatedTickets;
        }
    }
}
