using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using FacilitEase.Repositories;
using FacilitEase.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Dynamic.Core;
using FacilitEase.Repositories;
using System.Linq;

namespace FacilitEase.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IUnitOfWork _unitOfWork;//Avinash Abhijith
        private readonly AppDbContext _context;//Abhijith

        public TicketService(AppDbContext context, ITicketRepository ticketRepository, IDocumentRepository documentRepository, IUnitOfWork unitOfWork)
        {
            _context = context;
            _ticketRepository = ticketRepository;
            _documentRepository = documentRepository;
            _unitOfWork = unitOfWork;//Avinash
        }
        //Avinash
        public async Task<bool> ChangeTicketStatus(int ticketId, TicketStatusChangeRequest request)
        {
            try
            {
                var ticket = _unitOfWork.TicketRepository.GetById(ticketId);

                if (ticket == null)
                    return false;

                var newStatusId = request.IsApproved ? 3 : 2; // Set status based on IsApproved flag

                ticket.StatusId = newStatusId;

                /*_ticketRepository.Update(ticket);*/
                _unitOfWork.TicketRepository.Update(ticket);
                _unitOfWork.Complete();

                return true;
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return false;
            }
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
        public void TicketDecision(int ticketId, int statusId)
        {   
            var ticket = _unitOfWork.Ticket.GetById(ticketId);
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
        public void ChangePriority(int ticketId, int newPriorityId)
        {
            var ticket = _unitOfWork.Ticket.GetById(ticketId);
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
            var ticket = _unitOfWork.Ticket.GetById(ticketId);
            if (ticket == null)
            {
                throw new InvalidOperationException($"Ticket with ID {ticketId} not found.");
            }
            else
            {
                var manager = _unitOfWork.Employee.GetById(managerId);
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
        public void CreateTicketWithDocuments(TicketDto ticketDto)
        {

            var ticketEntity = new TBL_TICKET
            {
                TicketName = ticketDto.TicketName,
                TicketDescription = ticketDto.TicketDescription,
                PriorityId = ticketDto.PriorityId,
                CategoryId = ticketDto.CategoryId,
            };
            _context.Add(ticketEntity);

            _context.SaveChanges();


            foreach (var documentLink in ticketDto.DocumentLink)
            {
                var documentEntity = new TBL_DOCUMENT
                {
                    DocumentLink = documentLink,
                    TicketId = ticketEntity.Id,
                };

                _documentRepository.Add(documentEntity);
            }

            _context.SaveChanges();

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