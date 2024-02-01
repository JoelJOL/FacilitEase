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

        /// <summary>
        /// To create a new ticket along with associated documents in the database.
        /// </summary>
        /// <param name="ticketDto"></param>
        public void CreateTicketWithDocuments(TicketDto ticketDto)
        {


            var ticketEntity = new TBL_TICKET
            {
                TicketName = ticketDto.TicketName,
                TicketDescription = ticketDto.TicketDescription,
                PriorityId = ticketDto.PriorityId,
                CategoryId = ticketDto.CategoryId,
                StatusId = 1,
                CreatedBy = 1,
                UpdatedBy = 1, 
            };
            _context.Add(ticketEntity);

            _context.SaveChanges();


            foreach (var documentLink in ticketDto.DocumentLink)
            {
                var documentEntity = new TBL_DOCUMENT
                {
                    DocumentLink = documentLink,
                    TicketId = ticketEntity.Id,
                    CreatedBy = 1,
                    UpdatedBy = 1,
                };

                _documentRepository.Add(documentEntity);
            }

            _context.SaveChanges();

        }
        /// <summary>
        /// retrieve detailed information about a specific ticket
        /// </summary>
        /// <param name="desiredTicketId"></param>
        /// <returns></returns>
        public TicketDetails GetTicketDetails(int desiredTicketId)
            {
                         var ticketDetails = (from ticket in _context.TBL_TICKET
                         join user in _context.TBL_USER on ticket.UserId equals user.Id
                         join employee in _context.TBL_EMPLOYEE on user.EmployeeId equals employee.Id
                         join employeeDetail in _context.TBL_EMPLOYEE_DETAIL on employee.Id equals employeeDetail.EmployeeId
                         join location in _context.TBL_LOCATION on employeeDetail.LocationId equals location.Id
                         join department in _context.TBL_DEPARTMENT on employeeDetail.DepartmentId equals department.Id
                         join status in _context.TBL_STATUS on ticket.StatusId equals status.Id
                         join priority in _context.TBL_PRIORITY on ticket.PriorityId equals priority.Id
                         join document in _context.TBL_DOCUMENT on ticket.Id equals document.TicketId
                         join project in _context.TBL_PROJECT_EMPLOYEE_MAPPING on employee.Id equals project.EmployeeId
                         join projectcode in _context.TBL_PROJECT_CODE_GENERATION on project.ProjectId equals projectcode.ProjectId
                         join manager in _context.TBL_EMPLOYEE on employee.ManagerId equals manager.Id into managerJoin
                         from manager in managerJoin.DefaultIfEmpty()
                         where ticket.Id == desiredTicketId
                         select new TicketDetails
                         {
                             Id = ticket.Id,
                             TicketName = ticket.TicketName,
                             TicketDescription = ticket.TicketDescription,
                             StatusName = status.StatusName,
                             PriorityName = priority.PriorityName,
                             SubmittedDate = ticket.SubmittedDate,
                             RaisedEmployeeName = $"{employee.FirstName} {employee.LastName}",
                             ManagerName = manager != null ? $"{manager.FirstName} {manager.LastName}" : null,
                             ManagerId = employee.ManagerId,
                             LocationName = location.LocationName,
                             DeptName = department.DeptName,
                             DocumentLink = document.DocumentLink,
                             ProjectCode = projectcode.ProjectCode
                         })
                         .FirstOrDefault();

    return ticketDetails;
}

        /// <summary>
        /// retrieve unassigned tickets with optional search criteria
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="sortOrder"></param>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        public ManagerTicketResponse<UnassignedTicketModel> GetUnassignedTickets(int pageIndex, int pageSize, string sortField, string sortOrder, string searchQuery)
        {
            var unassignedTicketsQuery = _context.TBL_TICKET
                .Where(ticket => ticket.AssignedTo == null)
                .Where(ticket => string.IsNullOrEmpty(searchQuery) || ticket.TicketName.Contains(searchQuery))
                .Select(ticket => new UnassignedTicketModel
                {
                    Id = ticket.Id,
                    TicketName = ticket.TicketName,
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
                });

            var queryList = unassignedTicketsQuery.ToList();

            // Apply Sorting
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                string orderByString = $"{sortField} {sortOrder}";
                queryList = queryList.AsQueryable().OrderBy(orderByString).ToList();
            }

            // Apply Pagination
            var totalCount = queryList.Count();
            queryList = queryList.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            
            // Return the result in the ManagerTicketResponse format
            return new ManagerTicketResponse<UnassignedTicketModel>
            {
                Data = queryList,
                TotalDataCount = totalCount
            };
        }

        /// <summary>
        /// Assign the ticket to the agent
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="agentId"></param>
        public void AssignTicketToAgent(int ticketId, int agentId)
        {
            try
            {
                Console.WriteLine($"AssignTicketToAgent called with ticketId: {ticketId}, agentId: {agentId}");
                
                    // Find the ticket by ID
                    var ticket = _context.TBL_TICKET.FirstOrDefault(t => t.Id == ticketId);

                    if (ticket != null)
                    {
                        // Assign the ticket to the agent
                        ticket.AssignedTo = agentId;
                        ticket.UpdatedDate = DateTime.Now;

                        // Update the status to "In Progress"
                        ticket.StatusId = 2;

                        _context.SaveChanges();

                        Console.WriteLine($"Ticket {ticketId} assigned to agent {agentId} successfully.");
                    }
                    
         
                else
                {
                    Console.WriteLine($"Agent not found with ID: {agentId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw; 
            }
        }

        /// <summary>
        /// retrieve assigned tickets with optional search criteria
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="sortOrder"></param>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        public ManagerTicketResponse<TicketApiModel> GetAssignedTickets(int pageIndex, int pageSize, string sortField, string sortOrder, string searchQuery)
        {
            var assignedTicketsQuery = _context.TBL_TICKET
                .Where(ticket => ticket.AssignedTo != null)
                .Where(ticket => string.IsNullOrEmpty(searchQuery) || ticket.TicketName.Contains(searchQuery))
                .Select(ticket => new TicketApiModel
                {
                    Id = ticket.Id,
                    TicketName = ticket.TicketName,
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
                });

            var queryList = assignedTicketsQuery.ToList();

            // Apply Sorting
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                string orderByString = $"{sortField} {sortOrder}";
                queryList = queryList.AsQueryable().OrderBy(orderByString).ToList();
            }

            // Apply Pagination
            var totalCount = queryList.Count();
            queryList = queryList.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            // Return the result in the ManagerTicketResponse format
            return new ManagerTicketResponse<TicketApiModel>
            {
                Data = queryList,
                TotalDataCount = totalCount
            };
        }
        
        /// <summary>
        /// retrieve escalated tickets with optional search criteria
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="sortOrder"></param>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        public ManagerTicketResponse<TicketApiModel> GetEscalatedTickets(int pageIndex, int pageSize, string sortField, string sortOrder, string searchQuery)
        {
            var escalatedTicketsQuery = _context.TBL_TICKET
                .Join(_context.TBL_USER,
                    ticket => ticket.UserId,
                    user => user.Id,
                    (ticket, user) => new { Ticket = ticket, User = user })
                .Join(_context.TBL_EMPLOYEE,
                    joined => joined.User.EmployeeId,
                    employee => employee.Id,
                    (joined, employee) => new TicketApiModel
                    {
                        Id = joined.Ticket.Id,
                        TicketName = joined.Ticket.TicketName,
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
                    })
                .Where(ticket => ticket.Status == "Escalated")
                .Where(ticket => string.IsNullOrEmpty(searchQuery) || ticket.TicketName.Contains(searchQuery))
                .ToList();
            var queryList = escalatedTicketsQuery.ToList();

            // Apply Sorting
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                string orderByString = $"{sortField} {sortOrder}";
                queryList = queryList.AsQueryable().OrderBy(orderByString).ToList();
            }

            // Apply Pagination
            var totalCount = queryList.Count();
            queryList = queryList.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            // Return the result in the ManagerTicketResponse format
            return new ManagerTicketResponse<TicketApiModel>
            {
                Data = queryList,
                TotalDataCount = totalCount
            };
        }
    }

    
}