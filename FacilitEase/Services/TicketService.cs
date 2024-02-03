﻿using FacilitEase.Data;
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

        /// <summary>
        /// Get - Retrieves a list of tickets raised by the emloyees working under a specific manager and the total number of tickets 
        /// Includes pagination,searching and sorting functionality.
        /// </summary>
        /// <param name="managerId"></param>
        /// <param name="sortField"></param>
        /// <param name="sortOrder"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchQuery"></param>
        /// <returns>A response of paginated list of tickets of employees associated with a specific manager and the total tickets count</returns>
        public ManagerTicketResponse<ManagerEmployeeTickets> GetTicketByManager(int managerId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery)
        {
            var query = from ticket in _context.TBL_TICKET
                        join user in _context.TBL_USER on ticket.UserId equals user.Id
                        join employee in _context.TBL_EMPLOYEE on user.EmployeeId equals employee.Id
                        join priority in _context.TBL_PRIORITY on ticket.PriorityId equals priority.Id
                        join status in _context.TBL_STATUS on ticket.StatusId equals status.Id
                        where employee.ManagerId == managerId
                        where string.IsNullOrEmpty(searchQuery) || ticket.TicketName.Contains(searchQuery)
                        select new ManagerEmployeeTickets
                        {
                            Id = ticket.Id,
                            TicketName = ticket.TicketName,
                            EmployeeName = $"{employee.FirstName} {employee.LastName}",
                            AssignedTo = $"{_context.TBL_EMPLOYEE.Where(emp => emp.Id == ticket.AssignedTo).Select(emp => $"{emp.FirstName} {emp.LastName}").FirstOrDefault()}",
                            SubmittedDate = ticket.SubmittedDate,
                            Priority = $"{priority.PriorityName}",
                            Status = $"{status.StatusName}",
                        };

            var queryList = query.ToList();

            // Apply Sorting
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                string orderByString = $"{sortField} {sortOrder}";
                queryList = queryList.AsQueryable().OrderBy(orderByString).ToList();
            }

            // Apply Pagination
            var totalCount = query.Count();
            queryList = queryList.Skip(pageIndex * pageSize).Take(pageSize).ToList();



            return new ManagerTicketResponse<ManagerEmployeeTickets>
            {
                Data = queryList,
                TotalDataCount = totalCount
            };
        }



        /// <summary>
        /// Get - Retrieves all the data required when the manager accesses the detailed view of a specific employee ticket
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public ManagerEmployeeTicketDetailed ViewTicketDetails(int ticketId)
        {
            var ticket = from t in _context.TBL_TICKET
                         join u in _context.TBL_USER on t.UserId equals u.Id
                         join e in _context.TBL_EMPLOYEE on u.EmployeeId equals e.Id
                         join p in _context.TBL_PRIORITY on t.PriorityId equals p.Id
                         join s in _context.TBL_STATUS on t.StatusId equals s.Id
                         where t.Id == ticketId
                         select new ManagerEmployeeTicketDetailed
                         {
                             Id = t.Id,
                             TicketName = t.TicketName,
                             EmployeeName = $"{e.FirstName} {e.LastName}",
                             AssignedTo = $"{_context.TBL_EMPLOYEE.Where(emp => emp.Id == t.AssignedTo).Select(emp => $"{emp.FirstName} {emp.LastName}").FirstOrDefault()}",
                             SubmittedDate = t.SubmittedDate,
                             priorityName = p.PriorityName,
                             statusName = s.StatusName,
                             Notes = _context.TBL_COMMENT
                                 .Where(comment => comment.TicketId == ticketId && comment.Category == "Note")
                                 .Select(comment => comment.Text)
                                 .FirstOrDefault(),
                             LastUpdate = _context.TBL_COMMENT
                                 .Where(comment => comment.TicketId == ticketId)
                                 .OrderByDescending(comment => comment.UpdatedDate)
                                 .Select(comment => (comment.UpdatedDate != null)
                                     ? (DateTime.Now - comment.UpdatedDate).TotalMinutes < 60
                                         ? $"{(int)(DateTime.Now - comment.UpdatedDate).TotalMinutes}M ago"
                                         : (DateTime.Now - comment.UpdatedDate).TotalHours < 24
                                             ? $"{(int)(DateTime.Now - comment.UpdatedDate).TotalHours}H ago"
                                             : $"{(int)(DateTime.Now - comment.UpdatedDate).TotalDays}D ago"
                                     : null)
                                 .FirstOrDefault(),
                             TicketDescription = t.TicketDescription,
                             DocumentLink = string.Join(", ", _context.TBL_DOCUMENT
                                 .Where(documents => documents.TicketId == t.Id)
                                 .Select(document => document.DocumentLink)
                                 .ToList())
                         };

            return ticket.FirstOrDefault();
        }


        /// <summary>
        /// Get - Retrieves a list of tickets raised by the emloyees that need approval from the manager and the total number of waiting tickets 
        /// Includes pagination,searching and sorting functionality.
        /// </summary>
        /// <param name="managerId"></param>
        /// <param name="sortField"></param>
        /// <param name="sortOrder"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        public ManagerTicketResponse<ManagerEmployeeTickets> GetApprovalTicket(int managerId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery)
        {
            var query = from ticket in _context.TBL_TICKET
                        join user in _context.TBL_USER on ticket.UserId equals user.Id
                        join employee in _context.TBL_EMPLOYEE on user.EmployeeId equals employee.Id
                        join priority in _context.TBL_PRIORITY on ticket.PriorityId equals priority.Id
                        join status in _context.TBL_STATUS on ticket.StatusId equals status.Id
                        where ticket.ControllerId == managerId
                        where string.IsNullOrEmpty(searchQuery) || ticket.TicketName.Contains(searchQuery)
                        select new ManagerEmployeeTickets
                        {
                            Id = ticket.Id,
                            TicketName = ticket.TicketName,
                            EmployeeName = $"{employee.FirstName} {employee.LastName}",
                            AssignedTo = $"{_context.TBL_EMPLOYEE.Where(emp => emp.Id == ticket.AssignedTo).Select(emp => $"{emp.FirstName} {emp.LastName}").FirstOrDefault()}",
                            SubmittedDate = ticket.SubmittedDate,
                            Priority = $"{priority.PriorityName}",
                            Status = $"{status.StatusName}",
                        };
            var queryList = query.ToList();

            // Apply Sorting
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                string orderByString = $"{sortField} {sortOrder}";
                queryList = queryList.AsQueryable().OrderBy(orderByString).ToList();
            }

            // Apply Pagination
            var totalCount = query.Count();
            queryList = queryList.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            

            return new ManagerTicketResponse<ManagerEmployeeTickets>
            {
                Data = queryList,
                TotalDataCount = totalCount
            };
        }


        /// <summary>
        /// Post - Decides whether the ticket needs to be accepted or rejected
        /// Updates the status of a ticket to inprogress or cancelled and changes controller to the agent or null.
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="statusId"></param>
        /// <exception cref="InvalidOperationException"></exception>
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
                if (statusId == 2) //If ticket is accepted change the status id to 2 which is id of Status - Inprogress and set Controller id to Agent id
                    ticket.ControllerId = ticket.AssignedTo;
                else               //If ticket is rejected change the status id to 5 which is id of Status - Cancelled and set Controller id to null
                    ticket.ControllerId = null;
            }
            _unitOfWork.Complete();
        }


        /// <summary>
        /// Post - Updates the priority id according to the priority specified by the Manager
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="newPriorityId"></param>
        /// <exception cref="InvalidOperationException"></exception>
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

        /// <summary>
        /// Post - Updates the controller id to the department head id if the manager things higher authority approval is necessary
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="managerId"></param>
        /// <exception cref="InvalidOperationException"></exception>
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