using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using FacilitEase.Repositories;
using FacilitEase.UnitOfWork;
using System.Linq.Dynamic.Core;
using System.Linq;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net.Sockets;

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
                UpdateTicketTracking(ticket.Id, newStatusId, ticket.AssignedTo, ticket.ControllerId, ticket.CreatedDate, ticket.CreatedBy);
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
                        join employeedetail in _context.TBL_EMPLOYEE_DETAIL on employee.Id equals employeedetail.EmployeeId
                        join location in _context.TBL_LOCATION on employeedetail.LocationId equals location.Id
                        join priority in _context.TBL_PRIORITY on ticket.PriorityId equals priority.Id
                        join status in _context.TBL_STATUS on ticket.StatusId equals status.Id
                        where employee.ManagerId == managerId
                        where string.IsNullOrEmpty(searchQuery) || ticket.TicketName.Contains(searchQuery)
                        select new ManagerEmployeeTickets
                        {
                            Id = ticket.Id,
                            TicketName = ticket.TicketName,
                            EmployeeName = $"{employee.FirstName} {employee.LastName}",
                            Location = location.LocationName,
                            AssignedTo = $"{_context.TBL_EMPLOYEE.Where(emp => emp.Id == ticket.AssignedTo).Select(emp => $"{emp.FirstName} {emp.LastName}").FirstOrDefault()}",
                            SubmittedDate = ticket.SubmittedDate.ToString("yyyy-MM-dd hh:mm tt"),
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
        /// Get - Retrieves a list of tickets raised by the emloyees that are currently live,  working under a specific manager and the total number of tickets
        /// Includes pagination,searching and sorting functionality.
        /// </summary>
        /// <param name="managerId"></param>
        /// <param name="sortField"></param>
        /// <param name="sortOrder"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchQuery"></param>
        /// <returns>A response of paginated list of tickets of employees associated with a specific manager and the total tickets count</returns>
        public ManagerTicketResponse<ManagerEmployeeTickets> GetLiveTicketByManager(int managerId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery)
        {
            var query = from ticket in _context.TBL_TICKET
                        join user in _context.TBL_USER on ticket.UserId equals user.Id
                        join employee in _context.TBL_EMPLOYEE on user.EmployeeId equals employee.Id
                        join employeedetail in _context.TBL_EMPLOYEE_DETAIL on employee.Id equals employeedetail.EmployeeId
                        join location in _context.TBL_LOCATION on employeedetail.LocationId equals location.Id
                        join priority in _context.TBL_PRIORITY on ticket.PriorityId equals priority.Id
                        join status in _context.TBL_STATUS on ticket.StatusId equals status.Id
                        where employee.ManagerId == managerId
                        where ((status.Id != 4) && (status.Id != 5))
                        where string.IsNullOrEmpty(searchQuery) || ticket.TicketName.Contains(searchQuery)
                        select new ManagerEmployeeTickets
                        {
                            Id = ticket.Id,
                            TicketName = ticket.TicketName,
                            EmployeeName = $"{employee.FirstName} {employee.LastName}",
                            Location = location.LocationName,
                            AssignedTo = $"{_context.TBL_EMPLOYEE.Where(emp => emp.Id == ticket.AssignedTo).Select(emp => $"{emp.FirstName} {emp.LastName}").FirstOrDefault()}",
                            SubmittedDate = ticket.SubmittedDate.ToString("yyyy-MM-dd hh:mm tt"),
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
                             LastUpdate = GetTimeSinceLastUpdate(ticketId),
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
                        join employeedetail in _context.TBL_EMPLOYEE_DETAIL on employee.Id equals employeedetail.EmployeeId
                        join location in _context.TBL_LOCATION on employeedetail.LocationId equals location.Id
                        join priority in _context.TBL_PRIORITY on ticket.PriorityId equals priority.Id
                        join status in _context.TBL_STATUS on ticket.StatusId equals status.Id
                        where ticket.ControllerId == managerId
                        where string.IsNullOrEmpty(searchQuery) || ticket.TicketName.Contains(searchQuery)
                        select new ManagerEmployeeTickets
                        {
                            Id = ticket.Id,
                            TicketName = ticket.TicketName,
                            EmployeeName = $"{employee.FirstName} {employee.LastName}",
                            Location = location.LocationName,
                            AssignedTo = $"{_context.TBL_EMPLOYEE.Where(emp => emp.Id == ticket.AssignedTo).Select(emp => $"{emp.FirstName} {emp.LastName}").FirstOrDefault()}",
                            SubmittedDate = ticket.SubmittedDate.ToString("yyyy-MM-dd hh:mm tt"),
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
                if (statusId == 2)
                {
                    //If ticket is accepted change the status id to 2 which is id of Status - Inprogress and set Controller id to Agent id
                    ticket.ControllerId = ticket.AssignedTo;
                    UpdateTicketTracking(ticket.Id, (int)ticket.StatusId, ticket.AssignedTo, ticket.ControllerId, ticket.CreatedDate, ticket.CreatedBy);
                }
                else               //If ticket is rejected change the status id to 5 which is id of Status - Cancelled and set Controller id to null
                    ticket.ControllerId = null;
                UpdateTicketTracking(ticket.Id, (int)ticket.StatusId, ticket.AssignedTo, ticket.ControllerId, ticket.CreatedDate, ticket.CreatedBy);
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
                    UpdateTicketTracking(ticket.Id, (int)ticket.StatusId, ticket.AssignedTo, ticket.ControllerId, ticket.CreatedDate, ticket.CreatedBy);
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
                UserId = ticketDto.UserId,
                SubmittedDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedBy = ticketDto.CreatedBy,
                UpdatedBy = ticketDto.UpdatedBy,
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
            UpdateTicketTracking(ticketEntity.Id, 1, null, null, DateTime.Now, ticketEntity.CreatedBy);
        }

        public bool RequestToCancelTicket(int ticketId)
        {
            var ticket = _context.TBL_TICKET.FirstOrDefault(t => t.Id == ticketId);

            if (ticket == null || !IsValidTicketStatus(ticket.StatusId))
            {
                return false;
            }

            ticket.StatusId = (ticket.StatusId == 1) ? 5 : 7;
            ticket.ControllerId = ticket.AssignedTo;
            _context.SaveChanges();
            UpdateTicketTracking(ticket.Id, (int)ticket.StatusId, ticket.AssignedTo, ticket.ControllerId, ticket.CreatedDate, ticket.CreatedBy);
            return true;
        }

        private bool IsValidTicketStatus(int? statusId)
        {
            // Check if the status is open or in progress (1, 2, 3)
            return statusId.HasValue && (statusId == 1 || statusId == 2 || statusId == 3 || statusId == 6);
        }

        public IEnumerable<DocumentDto> GetDocumentsByTicketId(int ticketId)
        {
            var documents = _context.TBL_DOCUMENT
                .Where(d => d.TicketId == ticketId)
                .Select(d => new DocumentDto
                {
                    documentLink = d.DocumentLink.Replace("\\", "/")
                })
                .ToList();

            return documents;
        }

        /// <summary>
        /// Method to add field to to ticket tracking table
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="statusId"></param>
        /// <param name="assignedTo"></param>
        /// <param name="controllerId"></param>
        /// <param name="ticketRaisedTimestamp"></param>
        /// <param name="updatedBy"></param>
        public void UpdateTicketTracking(int ticketId, int statusId, int? assignedTo, int? controllerId, DateTime? ticketRaisedTimestamp, int createdBy)
        {
            var trackingEntry = new TBL_TICKET_TRACKING
            {
                TicketId = ticketId,
                TicketStatusId = statusId,
                AssignedTo = assignedTo,
                ApproverId = controllerId,
                TicketRaisedTimestamp = ticketRaisedTimestamp ?? DateTime.Now,
                CreatedBy = createdBy,
                UpdatedBy = createdBy,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            _context.TBL_TICKET_TRACKING.Add(trackingEntry);
            _context.SaveChanges();
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
                                     /*join project in _context.TBL_PROJECT_EMPLOYEE_MAPPING on employee.Id equals project.EmployeeId
                                     join projectcode in _context.TBL_PROJECT_CODE_GENERATION on project.ProjectId equals projectcode.ProjectId*/
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
                                         SubmittedDate = ticket.SubmittedDate.ToString("dd-MM-yy hh:mm tt"),
                                         EmployeeName = $"{employee.FirstName} {employee.LastName}",
                                         ManagerName = manager != null ? $"{manager.FirstName} {manager.LastName}" : null,
                                         ManagerId = employee.ManagerId,
                                         LocationName = location.LocationName,
                                         DeptName = department.DeptName,
                                         DocumentLink = "new",
                                         ProjectCode = 111,
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

                ticketDetails.LastUpdate = GetTimeSinceLastUpdate(desiredTicketId);
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
        public ManagerTicketResponse<UnassignedTicketModel> GetUnassignedTickets(int userId, int pageIndex, int pageSize, string sortField, string sortOrder, string searchQuery)
        {
            // Step 1: Retrieve DepartmentId based on UserId
            var departmentId = _context.TBL_EMPLOYEE_DETAIL
                .Where(employeeDetail => employeeDetail.EmployeeId == _context.TBL_USER
                    .Where(user => user.Id == userId)
                    .Select(user => user.EmployeeId)
                    .FirstOrDefault())
                .Select(employeeDetail => employeeDetail.DepartmentId)
                .FirstOrDefault();
            Console.WriteLine(departmentId);

            // Step 2: Get Categories corresponding to DepartmentId
            var categoriesForDepartment = _context.TBL_CATEGORY
                .Where(category => category.DepartmentId == departmentId)
                .Select(category => category.Id)
                .ToList();

            // Step 3: Filter unassigned tickets based on selected categories
            var unassignedTicketsQuery = _context.TBL_TICKET
     .Where(ticket => (ticket.AssignedTo == null || ticket.AssignedTo == 0) &&
                      categoriesForDepartment.Contains(ticket.CategoryId))
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
         SubmittedDate = ticket.SubmittedDate.ToString("yyyy-MM-dd hh:mm tt"),
         Priority = _context.TBL_PRIORITY
             .Where(priority => priority.Id == ticket.PriorityId)
             .Select(priority => priority.PriorityName)
             .FirstOrDefault(),
         Status = _context.TBL_STATUS
             .Where(status => status.Id == ticket.StatusId)
             .Select(status => status.StatusName)
             .FirstOrDefault(),
         Department = _context.TBL_USER
     .Where(user => user.Id == ticket.UserId)
     .Select(user => _context.TBL_EMPLOYEE
         .Where(employee => employee.Id == user.EmployeeId)
         .Select(employee => _context.TBL_EMPLOYEE_DETAIL
             .Where(employeeDetail => employeeDetail.Id == employee.Id)
             .Select(employeeDetail => _context.TBL_DEPARTMENT
                 .Where(department => department.Id == employeeDetail.DepartmentId)
                 .Select(department => department.DeptName)
                 .FirstOrDefault())
             .FirstOrDefault())
         .FirstOrDefault())
     .FirstOrDefault(),
         Location = _context.TBL_USER
     .Where(user => user.Id == ticket.UserId)
     .Select(user => _context.TBL_EMPLOYEE
         .Where(employee => employee.Id == user.EmployeeId)
         .Select(employee => _context.TBL_EMPLOYEE_DETAIL
             .Where(employeeDetail => employeeDetail.Id == employee.Id)
             .Select(employeeDetail => _context.TBL_LOCATION
                 .Where(location => location.Id == employeeDetail.LocationId)
                 .Select(location => location.LocationName)
                 .FirstOrDefault())
             .FirstOrDefault())
         .FirstOrDefault())
     .FirstOrDefault(),
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
        public void AssignTicketToAgent(int userId, int ticketId, int agentId)
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
                    ticket.ControllerId = agentId;
                    ticket.UpdatedDate = DateTime.Now;
                    ticket.UpdatedBy = userId;

                    // Update the status to "In Progress"
                    ticket.StatusId = 2;

                    // Add record to TBL_TICKET_ASSIGNMENT
                    TBL_TICKET_ASSIGNMENT ticketassign = new TBL_TICKET_ASSIGNMENT
                    {
                        TicketId = ticketId,
                        EmployeeId = agentId,  // Assuming agentId is the EmployeeId in TBL_EMPLOYEE
                        TicketAssignedTimestamp = DateTime.Now,
                        EmployeeStatus = "unresolved",
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now
                    };

                    _context.TBL_TICKET_ASSIGNMENT.Add(ticketassign);

                    _context.SaveChanges();

                    Console.WriteLine($"Ticket {ticketId} assigned to agent {agentId} successfully.");
                    UpdateTicketTracking(
                       ticket.Id, 2,
                       ticket.AssignedTo,
                       ticket.ControllerId,
                       ticket.UpdatedDate,
                       ticket.CreatedBy
                        );
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
        public ManagerTicketResponse<TicketApiModel> GetAssignedTickets(int userId, int pageIndex, int pageSize, string sortField, string sortOrder, string searchQuery)
        {
            // Step 1: Retrieve DepartmentId based on UserId
            var departmentId = _context.TBL_EMPLOYEE_DETAIL
                .Where(employeeDetail => employeeDetail.EmployeeId == _context.TBL_USER
                    .Where(user => user.Id == userId)
                    .Select(user => user.EmployeeId)
                    .FirstOrDefault())
                .Select(employeeDetail => employeeDetail.DepartmentId)
                .FirstOrDefault();

            // Step 2: Get Categories corresponding to DepartmentId
            var categoriesForDepartment = _context.TBL_CATEGORY
                .Where(category => category.DepartmentId == departmentId)
                .Select(category => category.Id)
                .ToList();

            // Step 3: Filter assigned tickets based on selected categories
            var assignedTicketsQuery = _context.TBL_TICKET
                .Where(ticket => ticket.AssignedTo != null &&
                                 categoriesForDepartment.Contains(ticket.CategoryId))
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
                    SubmittedDate = ticket.SubmittedDate.ToString("yyyy-MM-dd hh:mm tt"),
                    Priority = _context.TBL_PRIORITY
                        .Where(priority => priority.Id == ticket.PriorityId)
                        .Select(priority => priority.PriorityName)
                        .FirstOrDefault(),
                    Status = _context.TBL_STATUS
                        .Where(status => status.Id == ticket.StatusId)
                        .Select(status => status.StatusName)
                        .FirstOrDefault(),
                    Department = _context.TBL_USER
     .Where(user => user.Id == ticket.UserId)
     .Select(user => _context.TBL_EMPLOYEE
         .Where(employee => employee.Id == user.EmployeeId)
         .Select(employee => _context.TBL_EMPLOYEE_DETAIL
             .Where(employeeDetail => employeeDetail.Id == employee.Id)
             .Select(employeeDetail => _context.TBL_DEPARTMENT
                 .Where(department => department.Id == employeeDetail.DepartmentId)
                 .Select(department => department.DeptName)
                 .FirstOrDefault())
             .FirstOrDefault())
         .FirstOrDefault())
     .FirstOrDefault(),
                    Location = _context.TBL_USER
     .Where(user => user.Id == ticket.UserId)
     .Select(user => _context.TBL_EMPLOYEE
         .Where(employee => employee.Id == user.EmployeeId)
         .Select(employee => _context.TBL_EMPLOYEE_DETAIL
             .Where(employeeDetail => employeeDetail.Id == employee.Id)
             .Select(employeeDetail => _context.TBL_LOCATION
                 .Where(location => location.Id == employeeDetail.LocationId)
                 .Select(location => location.LocationName)
                 .FirstOrDefault())
             .FirstOrDefault())
         .FirstOrDefault())
     .FirstOrDefault(),
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
        public ManagerTicketResponse<TicketApiModel> GetEscalatedTickets(int userId, int pageIndex, int pageSize, string sortField, string sortOrder, string searchQuery)
        {
            // Step 1: Retrieve DepartmentId based on UserId
            var departmentId = _context.TBL_EMPLOYEE_DETAIL
                .Where(employeeDetail => employeeDetail.EmployeeId == _context.TBL_USER
                    .Where(user => user.Id == userId)
                    .Select(user => user.EmployeeId)
                    .FirstOrDefault())
                .Select(employeeDetail => employeeDetail.DepartmentId)
                .FirstOrDefault();

            // Step 2: Get Categories corresponding to DepartmentId
            var categoriesForDepartment = _context.TBL_CATEGORY
                .Where(category => category.DepartmentId == departmentId)
                .Select(category => category.Id)
                .ToList();

            // Step 3: Get the StatusId for "Escalated" from TBL_STATUS
            var escalatedStatusId = _context.TBL_STATUS
                .Where(status => status.StatusName == "Escalated")
                .Select(status => status.Id)
                .FirstOrDefault();

            // Step 4: Get the UserId of L3Admin role
            var l3AdminRoleId = _context.TBL_USER_ROLE
     .Where(role => role.UserRoleName == "L3Admin")
     .Select(role => role.Id)
     .FirstOrDefault();

            var l3AdminEmployeeIds = _context.TBL_USER_ROLE_MAPPING
                .Where(mapping => mapping.UserRoleId == l3AdminRoleId)
                .Join(_context.TBL_USER,
                    mapping => mapping.UserId,
                    user => user.Id,
                    (mapping, user) => user.EmployeeId)
                .ToList();

            // Step 5: Filter escalated tickets for L3Admin assignedTo
            var escalatedTicketsQuery = _context.TBL_TICKET
                .Where(ticket => ticket.AssignedTo != null)
                .Where(ticket => ticket.StatusId == escalatedStatusId)
                .Where(ticket => categoriesForDepartment.Contains(ticket.CategoryId))
                .Where(ticket => l3AdminEmployeeIds.Contains(ticket.AssignedTo.Value))
                .Where(ticket => string.IsNullOrEmpty(searchQuery) || ticket.TicketName.Contains(searchQuery))
                .Select(ticket => new FacilitEase.Models.ApiModels.TicketApiModel
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
                    SubmittedDate = ticket.SubmittedDate.ToString("yyyy-MM-dd hh:mm tt"),
                    Priority = _context.TBL_PRIORITY
                        .Where(priority => priority.Id == ticket.PriorityId)
                        .Select(priority => priority.PriorityName)
                        .FirstOrDefault(),
                    Status = _context.TBL_STATUS
                        .Where(status => status.Id == ticket.StatusId)
                        .Select(status => status.StatusName)
                        .FirstOrDefault(),
                    AssignedTo = _context.TBL_EMPLOYEE
                        .Where(employee => employee.Id == ticket.AssignedTo)
                        .Select(employee => $"{employee.FirstName} {employee.LastName}")
                        .FirstOrDefault(),
                    Department = _context.TBL_USER
                        .Where(user => user.Id == ticket.UserId)
                        .Select(user => _context.TBL_EMPLOYEE
                            .Where(employee => employee.Id == user.EmployeeId)
                            .Select(employee => _context.TBL_EMPLOYEE_DETAIL
                                .Where(employeeDetail => employeeDetail.Id == employee.Id)
                                .Select(employeeDetail => _context.TBL_DEPARTMENT
                                    .Where(department => department.Id == employeeDetail.DepartmentId)
                                    .Select(department => department.DeptName)
                                    .FirstOrDefault())
                                .FirstOrDefault())
                            .FirstOrDefault())
                        .FirstOrDefault(),
                    Location = _context.TBL_USER
                        .Where(user => user.Id == ticket.UserId)
                        .Select(user => _context.TBL_EMPLOYEE
                            .Where(employee => employee.Id == user.EmployeeId)
                            .Select(employee => _context.TBL_EMPLOYEE_DETAIL
                                .Where(employeeDetail => employeeDetail.Id == employee.Id)
                                .Select(employeeDetail => _context.TBL_LOCATION
                                    .Where(location => location.Id == employeeDetail.LocationId)
                                    .Select(location => location.LocationName)
                                    .FirstOrDefault())
                                .FirstOrDefault())
                            .FirstOrDefault())
                        .FirstOrDefault(),
                });

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
            var ticket = from t in _context.TBL_TICKET
                         join u in _context.TBL_USER on t.UserId equals u.Id
                         join e in _context.TBL_EMPLOYEE on u.EmployeeId equals e.Id
                         join p in _context.TBL_PRIORITY on t.PriorityId equals p.Id
                         join s in _context.TBL_STATUS on t.StatusId equals s.Id
                         // Additional join with manager information
                         join m in _context.TBL_EMPLOYEE on e.ManagerId equals m.Id
                         where t.Id == ticketId
                         select new DepartmentHeadManagerTicketDetails
                         {
                             Id = t.Id,
                             TicketName = t.TicketName,
                             EmployeeName = $"{e.FirstName} {e.LastName}",
                             // Extracting manager's name for "Forwarded By" field
                             ForwardedBy = $"{((e.ManagerId == e.Id) ? e.FirstName + " " + e.LastName : m.FirstName + " " + m.LastName)}",
                             AssignedTo = $"{_context.TBL_EMPLOYEE.Where(emp => emp.Id == t.AssignedTo).Select(emp => $"{emp.FirstName} {emp.LastName}").FirstOrDefault()}",
                             SubmittedDate = t.SubmittedDate.ToString("dd-MM-yy hh:mm tt"),
                             priorityName = p.PriorityName,
                             statusName = s.StatusName,
                             Notes = _context.TBL_COMMENT
                                 .Where(comment => comment.TicketId == ticketId && comment.Category == "Note")
                                 .Select(comment => comment.Text)
                                 .FirstOrDefault(),
                             LastUpdate = GetTimeSinceLastUpdate(ticketId),
                             TicketDescription = t.TicketDescription,
                             DocumentLink = string.Join(", ", _context.TBL_DOCUMENT
                                 .Where(documents => documents.TicketId == t.Id)
                                 .Select(document => document.DocumentLink)
                                 .ToList())
                         };

            return ticket.FirstOrDefault();
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
                .Select(ticket => new
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
                    Department = _context.TBL_USER
                        .Where(user => user.Id == ticket.UserId)
                        .Select(user => _context.TBL_EMPLOYEE
                            .Where(employee => employee.Id == user.EmployeeId)
                            .Select(employee => _context.TBL_EMPLOYEE_DETAIL
                                .Where(employeeDetail => employeeDetail.Id == employee.Id)
                                .Select(employeeDetail => _context.TBL_DEPARTMENT
                                    .Where(department => department.Id == employeeDetail.DepartmentId)
                                    .Select(department => department.DeptName)
                                    .FirstOrDefault())
                                .FirstOrDefault())
                            .FirstOrDefault())
                        .FirstOrDefault(),
                    Location = _context.TBL_USER
                        .Where(user => user.Id == ticket.UserId)
                        .Select(user => _context.TBL_EMPLOYEE
                            .Where(employee => employee.Id == user.EmployeeId)
                            .Select(employee => _context.TBL_EMPLOYEE_DETAIL
                                .Where(employeeDetail => employeeDetail.Id == employee.Id)
                                .Select(employeeDetail => _context.TBL_LOCATION
                                    .Where(location => location.Id == employeeDetail.LocationId)
                                    .Select(location => location.LocationName)
                                    .FirstOrDefault())
                                .FirstOrDefault())
                            .FirstOrDefault())
                        .FirstOrDefault(),
                });

            var queryList = tickets.ToList();

            // Apply Sorting
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                string orderByString = $"{sortField} {sortOrder}";
                queryList = queryList.AsQueryable().OrderBy(orderByString).ToList();
            }

            // Convert dates to string after sorting
            var finalQueryList = queryList.Select(q => new DepartmentHeadManagerTickets
            {
                Id = q.Id,
                TicketName = q.TicketName,
                EmployeeName = q.EmployeeName,
                AssignedTo = q.AssignedTo,
                SubmittedDate = q.SubmittedDate.ToString("dd-MM-yy hh:mm tt"),
                Priority = q.Priority,
                Status = q.Status,
                Department = q.Department,
                Location = q.Location,
            }).ToList();

            // Apply Pagination
            var totalCount = queryList.Count();
            finalQueryList = finalQueryList.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            return new DepartmentHeadTicketResponse<DepartmentHeadManagerTickets>
            {
                Data = finalQueryList,
                TotalDataCount = totalCount
            };
        }



        /// <summary>
        /// Method to find last comments last updated
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public string GetTimeSinceLastUpdate(int ticketId)
        {
            // Retrieving the comment related to the specified ticket ID.
            var comment = _context.TBL_COMMENT
                .FirstOrDefault(c => c.TicketId == ticketId);

            // Checking if a comment is found for the specified ticket ID.
            if (comment != null && comment.UpdatedDate != null)
            {
                // Calculate the time difference between CreatedDate and UpdatedDate.
                TimeSpan timeSinceLastUpdate = DateTime.Now - comment.UpdatedDate;

                // Format the time difference accordingly.
                if (timeSinceLastUpdate.TotalDays >= 1)
                {
                    return $"{(int)timeSinceLastUpdate.TotalDays} day(s) ago";
                }
                else if (timeSinceLastUpdate.TotalHours >= 1)
                {
                    return $"{(int)timeSinceLastUpdate.TotalHours} hour(s) ago";
                }
                else if (timeSinceLastUpdate.TotalMinutes >= 1)
                {
                    return $"{(int)timeSinceLastUpdate.TotalMinutes} minute(s) ago";
                }
                else
                {
                    return $"{(int)timeSinceLastUpdate.TotalSeconds} second(s) ago";
                }
            }

            return "No comment found for the specified ticket ID";
        }
    }
}