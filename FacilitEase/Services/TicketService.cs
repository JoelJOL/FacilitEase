using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using FacilitEase.Repositories;
using FacilitEase.UnitOfWork;
using System.Linq.Dynamic.Core;
using System.Linq;
using Microsoft.Net.Http.Headers;


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
        /// <summary>
        /// Changes the status of a ticket.
        /// </summary>
        /// <param name="ticketId">The ID of the ticket to change.</param>
        /// <param name="request">The request containing the new status.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating the success of the operation.</returns>
        public async Task<bool> ChangeTicketStatus(int ticketId, TicketStatusChangeRequest request)
        {
            try
            {
                var ticket = _unitOfWork.TicketRepository.GetById(ticketId);

                if (ticket == null)
                    return false;

                // Set status based on IsApproved flag
                var newStatusId = request.IsApproved ? 2 : 5;

                ticket.StatusId = newStatusId;

                // Set ControllerId based on IsApproved flag
                ticket.ControllerId = request.IsApproved ? ticket.AssignedTo : null;

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

        /// <summary> working
        /// To create a new ticket along with associated documents in the database.
        /// </summary>
        /// <param name="ticketDto"></param>
        public void CreateTicketWithDocuments(TicketDto ticketDto, IFormFile file)
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
                UserId = 1,
            };

            _context.Add(ticketEntity);
            _context.SaveChanges();

            if (file != null && file.Length > 0)
            {
                var folderName = Path.Combine("Resources", "Images");
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
                var fullPath = Path.Combine(folderName, uniqueFileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var documentEntity = new TBL_DOCUMENT
                {
                    DocumentLink = fullPath,
                    TicketId = ticketEntity.Id,
                    CreatedBy = 1,
                    UpdatedBy = 1,
                };

                _documentRepository.Add(documentEntity);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// retrieve detailed information about a specific ticket
        /// </summary>
        /// <param name="desiredTicketId"></param>
        /// <returns></returns>
        public TicketDetails GetTicketDetails(int desiredTicketId)
        {
            var ticketDetailsList = (from ticket in _context.TBL_TICKET
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
                                         EmployeeName = $"{employee.FirstName} {employee.LastName}",
                                         ManagerName = manager != null ? $"{manager.FirstName} {manager.LastName}" : null,
                                         ManagerId = employee.ManagerId,
                                         LocationName = location.LocationName,
                                         DeptName = department.DeptName,
                                         DocumentLink = document.DocumentLink,
                                     })
        .ToList();  // Materialize the main query first
            Console.WriteLine(ticketDetailsList);
            var ticketDetails = ticketDetailsList.FirstOrDefault();

            // Now execute subqueries separately
            if (ticketDetails != null)
            {
                ticketDetails.Notes = _context.TBL_COMMENT
                    .Where(comment => comment.TicketId == desiredTicketId && comment.Category == "Note")
                    .Select(comment => comment.Text)
                    .FirstOrDefault();

                ticketDetails.LastUpdate = _context.TBL_COMMENT
                    .Where(comment => comment.TicketId == desiredTicketId)
                    .OrderByDescending(comment => comment.UpdatedDate)
                    .Select(comment =>
                        (comment.UpdatedDate != null)
                            ? (DateTime.Now - comment.UpdatedDate).TotalMinutes < 60
                                ? $"{(int)(DateTime.Now - comment.UpdatedDate).TotalMinutes}M"
                                : (DateTime.Now - comment.UpdatedDate).TotalHours < 24
                                    ? $"{(int)(DateTime.Now - comment.UpdatedDate).TotalHours}H"
                                    : $"{(int)(DateTime.Now - comment.UpdatedDate).TotalDays}D"
                            : null
                    )
                    .FirstOrDefault();
            }

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

        //me
        /// <summary>
        /// Retrieves the details of a ticket for a department head or manager.
        /// </summary>
        /// <param name="ticketId">The ID of the ticket to retrieve.</param>
        /// <returns>A DepartmentHeadManagerTicketDetails object containing the detailed ticket view on selecting a particular ticket</returns>
        public DepartmentHeadManagerTicketDetails DHTicketDetails(int ticketId)
        {
            var ticket = _context.TBL_TICKET
                .Where(t => t.Id == ticketId)
                .Select(t => new DepartmentHeadManagerTicketDetails
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
                    priorityName = _context.TBL_PRIORITY
                        .Where(priority => priority.Id == t.PriorityId)
                        .Select(priority => priority.PriorityName)
                        .FirstOrDefault(),
                    statusName = _context.TBL_STATUS
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

        /// <summary>
        /// Retrieves a paginated list of tickets for approval by a department head.
        /// </summary>
        /// <param name="departmentHeadId">The ID of the department head.</param>
        /// <param name="sortField">The field to sort the tickets by.</param>
        /// <param name="sortOrder">The order to sort the tickets in.</param>
        /// <param name="pageIndex">The index of the page to retrieve.</param>
        /// <param name="pageSize">The size of the page to retrieve.</param>
        /// <param name="searchQuery">The query to filter the tickets by.</param>
        /// <returns>A DepartmentHeadTicketResponse object containing the paginated list of tickets and the total count of tickets.</returns>
        public DepartmentHeadTicketResponse<DepartmentHeadManagerTickets> DHGetApprovalTicket(int departmentHeadId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery)
        {
            var tickets = _context.TBL_TICKET
                .Where(ticket => ticket.ControllerId == departmentHeadId)
                .Where(ticket => string.IsNullOrEmpty(searchQuery) || ticket.TicketName.Contains(searchQuery))
                .Select(ticket => new DepartmentHeadManagerTickets
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

            var queryList = tickets.ToList();

            // Apply Sorting
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                string orderByString = $"{sortField} {sortOrder}";
                queryList = queryList.AsQueryable().OrderBy(orderByString).ToList();
            }

            // Apply Pagination
            var totalCount = queryList.Count();
            queryList = queryList.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            return new DepartmentHeadTicketResponse<DepartmentHeadManagerTickets>
            {
                Data = queryList,
                TotalDataCount = totalCount
            };
        }
    }
}