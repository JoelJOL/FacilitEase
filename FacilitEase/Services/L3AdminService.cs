using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using FacilitEase.UnitOfWork;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

namespace FacilitEase.Services
{
    public class L3AdminService : IL3AdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        private readonly ITicketService _ticketService;
        public L3AdminService(IUnitOfWork unitOfWork, AppDbContext context, ITicketService ticketService)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _ticketService = ticketService;
        }

        /// <summary>
        /// Method to change a particular ticket status to closed
        /// </summary>
        /// <param name="ticketId"></param>
        public void CloseTicket(int ticketId)
        {
            var ticketToClose = _context.TBL_TICKET
                .FirstOrDefault(t => t.Id == ticketId);

            if (ticketToClose != null)
            {
                ticketToClose.StatusId = 3;
                _context.SaveChanges();
                _ticketService.UpdateTicketTracking(
                ticketToClose.Id,3,
                ticketToClose.AssignedTo,
                ticketToClose.ControllerId,
                ticketToClose.SubmittedDate,
                ticketToClose.UpdatedBy
        );
            }
            
        }

        public void AcceptTicketCancellation(int ticketId)
        {
            var ticketToClose = _context.TBL_TICKET
                .FirstOrDefault(t => t.Id == ticketId);

            if (ticketToClose != null)
            {
                ticketToClose.StatusId = 3;
                _context.SaveChanges();
                _ticketService.UpdateTicketTracking(
            ticketToClose.Id,3,
            ticketToClose.AssignedTo,
            ticketToClose.ControllerId,
            ticketToClose.SubmittedDate,
            ticketToClose.UpdatedBy
        );
            }

        }

        public void DenyTicketCancellation(int ticketId)
        {
            var ticketToClose = _context.TBL_TICKET
                .FirstOrDefault(t => t.Id == ticketId);

            if (ticketToClose != null)
            {
                ticketToClose.StatusId = 2;

                _context.SaveChanges();
                _ticketService.UpdateTicketTracking(
            ticketToClose.Id,2,
            ticketToClose.AssignedTo,
            ticketToClose.ControllerId,
            ticketToClose.SubmittedDate,
            ticketToClose.UpdatedBy
        );
            }

        }

        /// <summary>
        /// // This method forwards a ticket to a specific department by updating relevant ticket details.
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="categoryId"></param>
        public void ForwardTicketToDept(int ticketId, int categoryId)
        {
            // Retrieve the department ID associated with the specified category.
            var category = _context.TBL_CATEGORY.FirstOrDefault(c => c.Id == categoryId);
            int deptId = category.DepartmentId;

            // Get the role ID associated with administrators.
            int adminRoleId = GetAdminRoleId();

            // Find an administrator in the specified department with the admin role. This isn't needed
            int adminEmployeeId = _context.TBL_USER_ROLE_MAPPING
                .Where(mapping => mapping.UserRoleId == adminRoleId && mapping.UserId != null)
                .Join(_context.TBL_USER_ROLE,
                    mapping => mapping.UserRoleId,
                    userRole => userRole.Id,
                    (mapping, userRole) => new { UserId = mapping.UserId, UserRoleName = userRole.UserRoleName })
                .Join(_context.TBL_USER,
                    userMapping => userMapping.UserId,
                    user => user.Id,
                    (userMapping, user) => new { UserId = userMapping.UserId, user.EmployeeId })
                .Join(_context.TBL_EMPLOYEE_DETAIL,
                    user => user.UserId,
                    employeeDetail => employeeDetail.EmployeeId,
                    (user, employeeDetail) => new { user.EmployeeId, employeeDetail.DepartmentId })
                .Where(result => result.DepartmentId == deptId)
                .Select(result => result.EmployeeId)
                .FirstOrDefault();

            // Retrieve the ticket to update.
            var ticketToUpdate = _context.TBL_TICKET
               .FirstOrDefault(t => t.Id == ticketId);

            // Check if the ticket exists.
            if (ticketToUpdate != null)
            {
                ticketToUpdate.StatusId = 1;
                ticketToUpdate.CategoryId = categoryId;
                ticketToUpdate.ControllerId = null;
                ticketToUpdate.AssignedTo = null;
                _context.SaveChanges();
            }

        }

        /// <summary>
        /// Method to get the ID of the Admin Role
        /// </summary>
        /// <returns></returns>
        public int GetAdminRoleId()
        {
            // Replace "Admin" with the actual role name for administrators
            var adminRole = _context.TBL_USER_ROLE
                .FirstOrDefault(role => role.UserRoleName == "Admin");

            return adminRole?.Id ?? 0;
        }


        /// <summary>
        /// Method to forward ticket to the manager of the employee who raised the ticket
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="managerId"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void ForwardTicket(int ticketId, int managerId)
        {
            var ticketToForward = _context.TBL_TICKET
                .FirstOrDefault(t => t.Id == ticketId);
            var manager = _context.TBL_EMPLOYEE.FirstOrDefault(e => e.Id == managerId);
            if (ticketToForward != null)
            {
                ticketToForward.StatusId = 5;

                if (manager?.Id != null)
                {
                    ticketToForward.ControllerId = managerId;
                    _context.SaveChanges();
                    _ticketService.UpdateTicketTracking(
                    ticketToForward.Id,5,
                    ticketToForward.AssignedTo,
                    ticketToForward.ControllerId,
                    ticketToForward.SubmittedDate,
                    ticketToForward.UpdatedBy
                );
                }
                else
                {
                    throw new InvalidOperationException("ManagerId is null or invalid.");
                }
                _context.SaveChanges();


            }

        }

        public void AddTicket(TBL_TICKET ticket)
        {

            _unitOfWork.Ticket.Add(ticket);
            _unitOfWork.Complete();
        }

        /// <summary>
        /// This method retrieves the notes associated with a specific ticket ID.
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public string GetCommentTextByTicketId(int ticketId)
        {
            var commentText = _context.TBL_COMMENT
                .Where(comment => comment.TicketId == ticketId)
                .Where(comment => comment.Category == "Note")
                .Select(comment => comment.Text)
                .FirstOrDefault();

            return commentText;
        }

        /// <summary>
        /// This method updates the comment text associated with a specific ticket ID.
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="newText"></param>
        public void UpdateCommentTextByTicketId(int ticketId, string newText)
        {
            // Retrieving the first comment related to the specified ticket ID.
            var comment = _context.TBL_COMMENT
                .FirstOrDefault(c => c.TicketId == ticketId);

            // Checking if a comment is found for the specified ticket ID.
            if (comment != null)
            {
                comment.Text = newText;
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// This method adds the comment of a particular ticket
        /// </summary>
        /// <param name="comment"></param>
        public void AddComment(TBL_COMMENT comment)
        {
            _context.TBL_COMMENT.Add(comment);
            _context.SaveChanges();
        }

        /// <summary>
        /// This method deletes the comment of a particular ticket
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>

        public async Task<bool> DeleteCommentAsync(int ticketId)
        {
            try
            {
                var comment = await _context.TBL_COMMENT.FirstOrDefaultAsync(c => c.TicketId == ticketId);

                if (comment != null)
                {
                    _context.TBL_COMMENT.Remove(comment);
                    await _context.SaveChangesAsync();
                    return true; // Successfully deleted
                }
                else
                {
                    return false; // Comment not found
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"Error deleting comment: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// This method retrieves a paginated list of tickets assigned to a specific l3 admin, with optional sorting and search functionality.
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="sortField"></param>
        /// <param name="sortOrder"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        public AgentTicketResponse<TicketJoin> GetTicketsByAgent(int agentId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery)
        {
            // Joining multiple tables to fetch necessary information about the tickets.
            var query = _context.TBL_TICKET
              .Join(
                  _context.TBL_USER,
                  ticket => ticket.UserId,
                  user => user.Id,
                  (ticket, user) => new { Ticket = ticket, User = user }
              )
              .Join(
                  _context.TBL_EMPLOYEE,
                  joined => joined.User.EmployeeId,
                  employee => employee.Id,
                  (joined, employee) => new { joined.Ticket, joined.User, Employee = employee }
              )
              .Join(
                  _context.TBL_PRIORITY,
                  joined => joined.Ticket.PriorityId,
                  priority => priority.Id,
                  (joined, priority) => new { joined.Ticket, joined.User, joined.Employee, Priority = priority }
              )
              .Join(
                  _context.TBL_STATUS,
                  joined => joined.Ticket.StatusId,
                  status => status.Id,
                  (joined, status) => new { joined.Ticket, joined.User, joined.Employee, joined.Priority, Status = status }
              )
              .Where(joined => joined.Ticket.AssignedTo == agentId && joined.Ticket.StatusId == 2)
              .Where(joined => string.IsNullOrEmpty(searchQuery) || joined.Ticket.TicketName.Contains(searchQuery))
              .Select(joined => new TicketJoin
              {
                  Id = joined.Ticket.Id,
                  TicketName = joined.Ticket.TicketName,
                  EmployeeName = $"{joined.Employee.FirstName} {joined.Employee.LastName}",
                  SubmittedDate = joined.Ticket.SubmittedDate,
                  Priority = joined.Priority.PriorityName,
                  Status = joined.Status.StatusName,
              });

            //Return the query data to a list.
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


            // Return the paginated and sorted ticket data along with the total count.
            return new AgentTicketResponse<TicketJoin>
            {
                Data = queryList,
                TotalDataCount = totalCount
            };
        }

        /// <summary>
        /// Retrieving detailed information about a specific ticket using LINQ query.
        /// </summary>
        /// <param name="desiredTicketId"></param>
        /// <returns></returns>

        public Join GetTicketDetailByAgent(int desiredTicketId)
        {
            var result = (from ticket in _context.TBL_TICKET
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
                          select new Join
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
                              ProjectCode = projectcode.ProjectCode
                          }).FirstOrDefault();

             return result;
        }


        /// <summary>
        /// This method retrieves resolved tickets assigned to a specific agent with optional sorting and search functionality.
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="sortField"></param>
        /// <param name="sortOrder"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        public AgentTicketResponse<TicketResolveJoin> GetResolvedTicketsByAgent(int agentId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery)
        {
            // Joining multiple tables to fetch necessary information about resolved tickets.
            var query = _context.TBL_TICKET
              .Join(
                  _context.TBL_USER,
                  ticket => ticket.UserId,
                  user => user.Id,
                  (ticket, user) => new { Ticket = ticket, User = user }
              )
              .Join(
                  _context.TBL_EMPLOYEE,
                  joined => joined.User.EmployeeId,
                  employee => employee.Id,
                  (joined, employee) => new { joined.Ticket, joined.User, Employee = employee }
              )
              .Join(
                  _context.TBL_PRIORITY,
                  joined => joined.Ticket.PriorityId,
                  priority => priority.Id,
                  (joined, priority) => new { joined.Ticket, joined.User, joined.Employee, Priority = priority }
              )
              .Join(
                  _context.TBL_STATUS,
                  joined => joined.Ticket.StatusId,
                  status => status.Id,
                  (joined, status) => new { joined.Ticket, joined.User, joined.Employee, joined.Priority, Status = status }
              )
              // Filtering resolved tickets based on agentId and StatusId.
              .Where(joined => joined.Ticket.AssignedTo == agentId && joined.Ticket.StatusId == 3)
              // Filtering resolved tickets based on searchQuery (if provided).
              .Where(joined => string.IsNullOrEmpty(searchQuery) || joined.Ticket.TicketName.Contains(searchQuery))
              // Selecting the desired fields and creating a new TicketResolveJoin object.
              .Select(joined => new TicketResolveJoin
              {
                  Id = joined.Ticket.Id,
                  TicketName = joined.Ticket.TicketName,
                  EmployeeName = $"{joined.Employee.FirstName} {joined.Employee.LastName}",
                  SubmittedDate = joined.Ticket.SubmittedDate,
                  ResolvedDate = joined.Ticket.UpdatedDate,
                  Priority = joined.Priority.PriorityName,

              });


            var materializedQuery = query.ToList();

            // Apply Sorting
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                string orderByString = $"{sortField} {sortOrder}";
                materializedQuery = materializedQuery.AsQueryable().OrderBy(orderByString).ToList();
            }

            // Apply Pagination
            var totalCount = materializedQuery.Count();
            materializedQuery = materializedQuery.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            // Return the paginated and sorted resolved ticket data along with the total count
            return new AgentTicketResponse<TicketResolveJoin>
            {
                Data = materializedQuery,
                TotalDataCount = totalCount
            };
        }

        /// <summary>
        /// Function that gets all tickets that have been escalated for that l3 admin
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public AgentTicketResponse<TicketResolveJoin> GetOnHoldTicketsByAgent(int agentId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery)
        {
            // Joining multiple tables to fetch necessary information about resolved tickets.
            var query = _context.TBL_TICKET
              .Join(
                  _context.TBL_USER,
                  ticket => ticket.UserId,
                  user => user.Id,
                  (ticket, user) => new { Ticket = ticket, User = user }
              )
              .Join(
                  _context.TBL_EMPLOYEE,
                  joined => joined.User.EmployeeId,
                  employee => employee.Id,
                  (joined, employee) => new { joined.Ticket, joined.User, Employee = employee }
              )
              .Join(
                  _context.TBL_PRIORITY,
                  joined => joined.Ticket.PriorityId,
                  priority => priority.Id,
                  (joined, priority) => new { joined.Ticket, joined.User, joined.Employee, Priority = priority }
              )
              .Join(
                  _context.TBL_STATUS,
                  joined => joined.Ticket.StatusId,
                  status => status.Id,
                  (joined, status) => new { joined.Ticket, joined.User, joined.Employee, joined.Priority, Status = status }
              )
              // Filtering resolved tickets based on agentId and StatusId.
              .Where(joined => joined.Ticket.AssignedTo == agentId && joined.Ticket.StatusId == 5)
              // Filtering resolved tickets based on searchQuery (if provided).
              .Where(joined => string.IsNullOrEmpty(searchQuery) || joined.Ticket.TicketName.Contains(searchQuery))
              // Selecting the desired fields and creating a new TicketResolveJoin object.
              .Select(joined => new TicketResolveJoin
              {
                  Id = joined.Ticket.Id,
                  TicketName = joined.Ticket.TicketName,
                  EmployeeName = $"{joined.Employee.FirstName} {joined.Employee.LastName}",
                  SubmittedDate = joined.Ticket.SubmittedDate,
                  ResolvedDate = joined.Ticket.UpdatedDate,
                  Priority = joined.Priority.PriorityName,

              });


            var materializedQuery = query.ToList();

            // Apply Sorting
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                string orderByString = $"{sortField} {sortOrder}";
                materializedQuery = materializedQuery.AsQueryable().OrderBy(orderByString).ToList();
            }

            // Apply Pagination
            var totalCount = materializedQuery.Count();
            materializedQuery = materializedQuery.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            // Return the paginated and sorted resolved ticket data along with the total count
            return new AgentTicketResponse<TicketResolveJoin>
            {
                Data = materializedQuery,
                TotalDataCount = totalCount
            };
        }

        public AgentTicketResponse<TicketResolveJoin> GetCancelRequestTicketsByAgent(int agentId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery)
        {
            // Joining multiple tables to fetch necessary information about resolved tickets.
            var query = _context.TBL_TICKET
              .Join(
                  _context.TBL_USER,
                  ticket => ticket.UserId,
                  user => user.Id,
                  (ticket, user) => new { Ticket = ticket, User = user }
              )
              .Join(
                  _context.TBL_EMPLOYEE,
                  joined => joined.User.EmployeeId,
                  employee => employee.Id,
                  (joined, employee) => new { joined.Ticket, joined.User, Employee = employee }
              )
              .Join(
                  _context.TBL_PRIORITY,
                  joined => joined.Ticket.PriorityId,
                  priority => priority.Id,
                  (joined, priority) => new { joined.Ticket, joined.User, joined.Employee, Priority = priority }
              )
              .Join(
                  _context.TBL_STATUS,
                  joined => joined.Ticket.StatusId,
                  status => status.Id,
                  (joined, status) => new { joined.Ticket, joined.User, joined.Employee, joined.Priority, Status = status }
              )
              // Filtering resolved tickets based on agentId and StatusId.
              .Where(joined => joined.Ticket.AssignedTo == agentId && joined.Ticket.StatusId == 6)
              // Filtering resolved tickets based on searchQuery (if provided).
              .Where(joined => string.IsNullOrEmpty(searchQuery) || joined.Ticket.TicketName.Contains(searchQuery))
              // Selecting the desired fields and creating a new TicketResolveJoin object.
              .Select(joined => new TicketResolveJoin
              {
                  Id = joined.Ticket.Id,
                  TicketName = joined.Ticket.TicketName,
                  EmployeeName = $"{joined.Employee.FirstName} {joined.Employee.LastName}",
                  SubmittedDate = joined.Ticket.SubmittedDate,
                  ResolvedDate = joined.Ticket.UpdatedDate,
                  Priority = joined.Priority.PriorityName,

              });


            var materializedQuery = query.ToList();

            // Apply Sorting
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                string orderByString = $"{sortField} {sortOrder}";
                materializedQuery = materializedQuery.AsQueryable().OrderBy(orderByString).ToList();
            }

            // Apply Pagination
            var totalCount = materializedQuery.Count();
            materializedQuery = materializedQuery.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            // Return the paginated and sorted resolved ticket data along with the total count
            return new AgentTicketResponse<TicketResolveJoin>
            {
                Data = materializedQuery,
                TotalDataCount = totalCount
            };
        }

        /// <summary>
        /// Method to retrieve all data from ticket tracking table
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public List<TrackingDetailsDto> GetTicketDetails(int ticketId)
        {
            var result = _context.TBL_TICKET_TRACKING
                .Where(tracking => tracking.TicketId == ticketId)
                .Select(tracking => new TrackingDetailsDto
                {
                    TicketId = tracking.TicketId,
                    StatusName = _context.TBL_STATUS
                                    .Where(status => status.Id == tracking.TicketStatusId)
                                    .Select(status => status.StatusName)
                                    .FirstOrDefault(),
                    SubmittedByEmployeeName = _context.TBL_EMPLOYEE
                        .Where(employee => employee.Id == tracking.CreatedBy)
                        .Select(employee => $"{employee.FirstName} {employee.LastName}")
                        .FirstOrDefault() ?? "Not Assigned",
                    AssignedToEmployeeName = _context.TBL_EMPLOYEE
                        .Where(employee => employee.Id == tracking.AssignedTo)
                        .Select(employee => $"{employee.FirstName} {employee.LastName}")
                        .FirstOrDefault() ?? "Not Assigned",
                    ApproverEmployeeName = _context.TBL_EMPLOYEE
                        .Where(employee => employee.Id == tracking.ApproverId)
                        .Select(employee => $"{employee.FirstName} {employee.LastName}")
                        .FirstOrDefault() ?? "Not Assigned",
                    TrackingCreatedDate = tracking.CreatedDate
                    // Add other properties as needed
                })
                .ToList();

            return result;
        }




    }
}