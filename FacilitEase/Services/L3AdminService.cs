﻿using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using FacilitEase.UnitOfWork;
using Microsoft.EntityFrameworkCore;
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
                ticketToClose.StatusId = 4;
                ticketToClose.UpdatedDate = DateTime.Now;
                _context.SaveChanges();
                _ticketService.UpdateTicketTracking(
                ticketToClose.Id, 4,
                ticketToClose.AssignedTo,
                ticketToClose.ControllerId,
                ticketToClose.SubmittedDate,
                ticketToClose.CreatedBy
        );
                //For Updating ticket tracking table
                var ticketassign = (from ta in _context.TBL_TICKET_ASSIGNMENT
                                    where ta.TicketId == ticketId
                                    select ta).FirstOrDefault();
                if (ticketassign != null)
                {
                    ticketassign.EmployeeStatus = "resolved";
                    _context.SaveChanges();
                }
            }
        }

        public void AcceptTicketCancellation(int ticketId)
        {
            var ticketToClose = _context.TBL_TICKET
                .FirstOrDefault(t => t.Id == ticketId);

            if (ticketToClose != null)
            {
                ticketToClose.StatusId = 5;
                ticketToClose.UpdatedDate = DateTime.Now;
                _context.SaveChanges();
                _ticketService.UpdateTicketTracking(
            ticketToClose.Id, 5,
            ticketToClose.AssignedTo,
            ticketToClose.ControllerId,
            ticketToClose.SubmittedDate,
            ticketToClose.CreatedBy
        );
                //For Updating ticket tracking table
                var ticketassign = (from ta in _context.TBL_TICKET_ASSIGNMENT
                                    where ta.Id == ticketId
                                    select ta).FirstOrDefault();
                if (ticketassign != null)
                {
                    ticketassign.EmployeeStatus = "resolved";
                    _context.SaveChanges();
                }
            }
        }

        public void DenyTicketCancellation(int ticketId)
        {
            var ticketToClose = _context.TBL_TICKET
                .FirstOrDefault(t => t.Id == ticketId);

            if (ticketToClose != null)
            {
                ticketToClose.StatusId = 2;
                ticketToClose.UpdatedDate = DateTime.Now;
                _context.SaveChanges();
                _ticketService.UpdateTicketTracking(
            ticketToClose.Id, 2,
            ticketToClose.AssignedTo,
            ticketToClose.ControllerId,
            ticketToClose.SubmittedDate,
            ticketToClose.CreatedBy
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
                ticketToUpdate.UpdatedDate = DateTime.Now;
                _context.SaveChanges();
                _ticketService.UpdateTicketTracking(
                   ticketToUpdate.Id, 1,
                   ticketToUpdate.AssignedTo,
                   ticketToUpdate.ControllerId,
                   ticketToUpdate.SubmittedDate,
                   ticketToUpdate.CreatedBy
                    );
            }
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
                ticketToForward.StatusId = 6;

                if (manager?.Id != null)
                {
                    ticketToForward.ControllerId = managerId;
                    ticketToForward.UpdatedDate = DateTime.Now;
                    _context.SaveChanges();
                    _ticketService.UpdateTicketTracking(
                    ticketToForward.Id, 6,
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
                comment.UpdatedDate = DateTime.Now;
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
        public AgentTicketResponse<RaisedTicketsDto> GetTicketsByAgent(int userId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery)
        {
            int agentId = _context.TBL_USER
           .Where(user => user.Id == userId)
           .Select(user => user.EmployeeId)
           .FirstOrDefault();
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
                  _context.TBL_EMPLOYEE_DETAIL,
                  joined => joined.Employee.Id,
                  detail => detail.Id,
                  (joined, detail) => new { joined.Ticket, joined.User, joined.Employee, Detail = detail }
              )
              .Join(
                  _context.TBL_DEPARTMENT,
                  joined => joined.Detail.DepartmentId,
                  department => department.Id,
                  (joined, department) => new { joined.Ticket, joined.User, joined.Employee, joined.Detail, Department = department }
              )
              .Join(
                  _context.TBL_LOCATION,
                  joined => joined.Detail.LocationId,
                  location => location.Id,
                  (joined, location) => new { joined.Ticket, joined.User, joined.Employee, joined.Detail, joined.Department, Location = location }
              )
              .Join(
                  _context.TBL_PRIORITY,
                  joined => joined.Ticket.PriorityId,
                  priority => priority.Id,
                  (joined, priority) => new { joined.Ticket, joined.User, joined.Employee, joined.Detail, joined.Department, joined.Location, Priority = priority }
              )
              .Join(
                  _context.TBL_STATUS,
                  joined => joined.Ticket.StatusId,
                  status => status.Id,
                  (joined, status) => new { joined.Ticket, joined.User, joined.Employee, joined.Detail, joined.Department, joined.Location, joined.Priority, Status = status }
              )
              // Filtering resolved tickets based on agentId and StatusId.
              .Where(joined => joined.Ticket.ControllerId == agentId && joined.Ticket.StatusId == 2)
              // Filtering resolved tickets based on searchQuery (if provided).
              .Where(joined => string.IsNullOrEmpty(searchQuery) || joined.Ticket.TicketName.Contains(searchQuery))
              // Selecting the desired fields and creating a new TicketResolveJoin object.
              .Select(joined => new 
              {
                  Id = joined.Ticket.Id,
                  TicketName = joined.Ticket.TicketName,
                  EmployeeName = $"{joined.Employee.FirstName} {joined.Employee.LastName}",
                  SubmittedDate = joined.Ticket.SubmittedDate,
                  Priority = joined.Priority.PriorityName,
                  Status = joined.Status.StatusName,
                  Department = joined.Department.DeptName,
                  Location = joined.Location.LocationName,

              });


            var queryList = query.ToList();

            // Apply Sorting
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                string orderByString = $"{sortField} {sortOrder}";
                queryList = queryList.AsQueryable().OrderBy(orderByString).ToList();
            }

            // Convert dates to string after sorting
            var finalQueryList = queryList.Select(q => new RaisedTicketsDto
            {
                Id = q.Id,
                TicketName = q.TicketName,
                EmployeeName = q.EmployeeName,
                Location = q.Location,
                Department = q.Department,
                SubmittedDate = q.SubmittedDate.ToString("yyyy-MM-dd hh:mm tt"),
                Priority = q.Priority,
                Status = q.Status,
            }).ToList();

            // Apply Pagination
            var totalCount = finalQueryList.Count();
            finalQueryList = finalQueryList.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            // Return the paginated and sorted resolved ticket data along with the total count
            return new AgentTicketResponse<RaisedTicketsDto>
            {
                Data = finalQueryList,
                TotalDataCount = totalCount
            };
        }

        /// <summary>
        /// Retrieving detailed information about a specific ticket using LINQ query.
        /// </summary>
        /// <param name="desiredTicketId"></param>
        /// <returns></returns>

        public TicketDetailDataDto GetTicketDetailByAgent(int desiredTicketId)
        {
            var result = (from ticket in _context.TBL_TICKET
                          join user in _context.TBL_USER on ticket.UserId equals user.Id
                          join employee in _context.TBL_EMPLOYEE on user.EmployeeId equals employee.Id
                          join employeeDetail in _context.TBL_EMPLOYEE_DETAIL on employee.Id equals employeeDetail.EmployeeId
                          join location in _context.TBL_LOCATION on employeeDetail.LocationId equals location.Id
                          join department in _context.TBL_DEPARTMENT on employeeDetail.DepartmentId equals department.Id
                          join status in _context.TBL_STATUS on ticket.StatusId equals status.Id
                          join priority in _context.TBL_PRIORITY on ticket.PriorityId equals priority.Id
                          join manager in _context.TBL_EMPLOYEE on employee.ManagerId equals manager.Id into managerJoin
                          from manager in managerJoin.DefaultIfEmpty()
                          where ticket.Id == desiredTicketId
                          select new TicketDetailDataDto
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
                          }).FirstOrDefault();
            if (result != null)
            {
                result.Notes = _context.TBL_COMMENT
                    .Where(comment => comment.TicketId == desiredTicketId && comment.Category == "Note")
                    .Select(comment => comment.Text)
                    .FirstOrDefault();
                if (string.IsNullOrEmpty(result.Notes))
                {
                    result.Notes = "No notes provided";
                }

                result.LastUpdate = GetTimeSinceLastUpdate(desiredTicketId);
            }
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
        public AgentTicketResponse<ResolvedTicketDto> GetResolvedTicketsByAgent(int userId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery)
        {
            int agentId = _context.TBL_USER
            .Where(user => user.Id == userId)
            .Select(user => user.EmployeeId)
            .FirstOrDefault();
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
                  _context.TBL_EMPLOYEE_DETAIL,
                  joined => joined.Employee.Id,
                  detail => detail.Id,
                  (joined, detail) => new { joined.Ticket, joined.User, joined.Employee, Detail = detail }
              )
              .Join(
                  _context.TBL_DEPARTMENT,
                  joined => joined.Detail.DepartmentId,
                  department => department.Id,
                  (joined, department) => new { joined.Ticket, joined.User, joined.Employee, joined.Detail, Department = department }
              )
              .Join(
                  _context.TBL_LOCATION,
                  joined => joined.Detail.LocationId,
                  location => location.Id,
                  (joined, location) => new { joined.Ticket, joined.User, joined.Employee, joined.Detail, joined.Department, Location = location }
              )
              .Join(
                  _context.TBL_PRIORITY,
                  joined => joined.Ticket.PriorityId,
                  priority => priority.Id,
                  (joined, priority) => new { joined.Ticket, joined.User, joined.Employee, joined.Detail, joined.Department, joined.Location, Priority = priority }
              )
              .Join(
                  _context.TBL_STATUS,
                  joined => joined.Ticket.StatusId,
                  status => status.Id,
                  (joined, status) => new { joined.Ticket, joined.User, joined.Employee, joined.Detail, joined.Department, joined.Location, joined.Priority, Status = status }
              )
              // Filtering resolved tickets based on agentId and StatusId.
              .Where(joined => joined.Ticket.ControllerId == agentId && joined.Ticket.StatusId == 4)
              // Filtering resolved tickets based on searchQuery (if provided).
              .Where(joined => string.IsNullOrEmpty(searchQuery) || joined.Ticket.TicketName.Contains(searchQuery))
              // Selecting the desired fields and creating a new TicketResolveJoin object.
              .Select(joined => new 
              {
                  Id = joined.Ticket.Id,
                  TicketName = joined.Ticket.TicketName,
                  EmployeeName = $"{joined.Employee.FirstName} {joined.Employee.LastName}",
                  SubmittedDate = joined.Ticket.SubmittedDate,
                  ResolvedDate = joined.Ticket.UpdatedDate,
                  Priority = joined.Priority.PriorityName,
                  Status = joined.Status.StatusName,
                  Department = joined.Department.DeptName,
                  Location = joined.Location.LocationName,


              });

            var queryList = query.ToList();

            // Apply Sorting
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                string orderByString = $"{sortField} {sortOrder}";
                queryList = queryList.AsQueryable().OrderBy(orderByString).ToList();
            }

            // Convert dates to string after sorting
            var finalQueryList = queryList.Select(q => new ResolvedTicketDto
            {
                Id = q.Id,
                TicketName = q.TicketName,
                EmployeeName = q.EmployeeName,
                Location = q.Location,
                Department = q.Department,
                SubmittedDate = q.SubmittedDate.ToString("yyyy-MM-dd hh:mm tt"),
                ResolvedDate = q.ResolvedDate.ToString("yyyy-MM-dd hh:mm tt"),
                Priority = q.Priority,
                Status = q.Status,
            }).ToList();

            // Apply Pagination
            var totalCount = finalQueryList.Count();
            finalQueryList = finalQueryList.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            // Return the paginated and sorted resolved ticket data along with the total count
            return new AgentTicketResponse<ResolvedTicketDto>
            {
                Data = finalQueryList,
                TotalDataCount = totalCount
            };
        }

        /// <summary>
        /// Function that gets all tickets that have been escalated for that l3 admin
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public AgentTicketResponse<ResolvedTicketDto> GetOnHoldTicketsByAgent(int userId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery)
        {
            int agentId = _context.TBL_USER
            .Where(user => user.Id == userId)
            .Select(user => user.EmployeeId)
            .FirstOrDefault();
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
                  _context.TBL_EMPLOYEE_DETAIL,
                  joined => joined.Employee.Id,
                  detail => detail.Id,
                  (joined, detail) => new { joined.Ticket, joined.User, joined.Employee, Detail = detail }
              )
              .Join(
                  _context.TBL_DEPARTMENT,
                  joined => joined.Detail.DepartmentId,
                  department => department.Id,
                  (joined, department) => new { joined.Ticket, joined.User, joined.Employee, joined.Detail, Department = department }
              )
              .Join(
                  _context.TBL_LOCATION,
                  joined => joined.Detail.LocationId,
                  location => location.Id,
                  (joined, location) => new { joined.Ticket, joined.User, joined.Employee, joined.Detail, joined.Department, Location = location }
              )
              .Join(
                  _context.TBL_PRIORITY,
                  joined => joined.Ticket.PriorityId,
                  priority => priority.Id,
                  (joined, priority) => new { joined.Ticket, joined.User, joined.Employee, joined.Detail, joined.Department, joined.Location, Priority = priority }
              )
              .Join(
                  _context.TBL_STATUS,
                  joined => joined.Ticket.StatusId,
                  status => status.Id,
                  (joined, status) => new { joined.Ticket, joined.User, joined.Employee, joined.Detail, joined.Department, joined.Location, joined.Priority, Status = status }
              )
              // Filtering resolved tickets based on agentId and StatusId.
              .Where(joined => joined.Ticket.AssignedTo == agentId && joined.Ticket.StatusId == 6)
              // Filtering resolved tickets based on searchQuery (if provided).
              .Where(joined => string.IsNullOrEmpty(searchQuery) || joined.Ticket.TicketName.Contains(searchQuery))
              // Selecting the desired fields and creating a new TicketResolveJoin object.
              .Select(joined => new 
              {
                  Id = joined.Ticket.Id,
                  TicketName = joined.Ticket.TicketName,
                  EmployeeName = $"{joined.Employee.FirstName} {joined.Employee.LastName}",
                  SubmittedDate = joined.Ticket.SubmittedDate,
                  ResolvedDate = joined.Ticket.UpdatedDate,
                  Priority = joined.Priority.PriorityName,
                  Status = joined.Status.StatusName,
                  Department = joined.Department.DeptName,
                  Location = joined.Location.LocationName,

              });

            var queryList = query.ToList();

            // Apply Sorting
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                string orderByString = $"{sortField} {sortOrder}";
                queryList = queryList.AsQueryable().OrderBy(orderByString).ToList();
            }

            // Convert dates to string after sorting
            var finalQueryList = queryList.Select(q => new ResolvedTicketDto
            {
                Id = q.Id,
                TicketName = q.TicketName,
                EmployeeName = q.EmployeeName,
                Location = q.Location,
                Department = q.Department,
                SubmittedDate = q.SubmittedDate.ToString("yyyy-MM-dd hh:mm tt"),
                ResolvedDate = q.ResolvedDate.ToString("yyyy-MM-dd hh:mm tt"),
                Priority = q.Priority,
                Status = q.Status,
            }).ToList();

            // Apply Pagination
            var totalCount = finalQueryList.Count();
            finalQueryList = finalQueryList.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            // Return the paginated and sorted resolved ticket data along with the total count
            return new AgentTicketResponse<ResolvedTicketDto>
            {
                Data = finalQueryList,
                TotalDataCount = totalCount
            };
        }

        public AgentTicketResponse<ResolvedTicketDto> GetCancelRequestTicketsByAgent(int userId, string sortField, string sortOrder, int pageIndex, int pageSize, string searchQuery)
        {
                int agentId = _context.TBL_USER
            .Where(user => user.Id == userId)
            .Select(user => user.EmployeeId)
            .FirstOrDefault();
            // Joining multiple tables to fetch necessary information about cancellation request tickets.
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
                  _context.TBL_EMPLOYEE_DETAIL,
                  joined => joined.Employee.Id,
                  detail => detail.Id,
                  (joined, detail) => new { joined.Ticket, joined.User, joined.Employee, Detail = detail }
              )
              .Join(
                  _context.TBL_DEPARTMENT,
                  joined => joined.Detail.DepartmentId,
                  department => department.Id,
                  (joined, department) => new { joined.Ticket, joined.User, joined.Employee, joined.Detail, Department = department }
              )
              .Join(
                  _context.TBL_LOCATION,
                  joined => joined.Detail.LocationId,
                  location => location.Id,
                  (joined, location) => new { joined.Ticket, joined.User, joined.Employee, joined.Detail, joined.Department, Location = location }
              )
              .Join(
                  _context.TBL_PRIORITY,
                  joined => joined.Ticket.PriorityId,
                  priority => priority.Id,
                  (joined, priority) => new { joined.Ticket, joined.User, joined.Employee, joined.Detail, joined.Department, joined.Location, Priority = priority }
              )
              .Join(
                  _context.TBL_STATUS,
                  joined => joined.Ticket.StatusId,
                  status => status.Id,
                  (joined, status) => new { joined.Ticket, joined.User, joined.Employee, joined.Detail, joined.Department, joined.Location, joined.Priority, Status = status }
              )
              // Filtering resolved tickets based on agentId and StatusId.
              .Where(joined => joined.Ticket.ControllerId == agentId && joined.Ticket.StatusId == 7)
              // Filtering resolved tickets based on searchQuery (if provided).
              .Where(joined => string.IsNullOrEmpty(searchQuery) || joined.Ticket.TicketName.Contains(searchQuery))
              // Selecting the desired fields and creating a new TicketResolveJoin object.
              .Select(joined => new 
              {
                  Id = joined.Ticket.Id,
                  TicketName = joined.Ticket.TicketName,
                  EmployeeName = $"{joined.Employee.FirstName} {joined.Employee.LastName}",
                  SubmittedDate = joined.Ticket.SubmittedDate,
                  ResolvedDate = joined.Ticket.UpdatedDate,
                  Priority = joined.Priority.PriorityName,
                  Status = joined.Status.StatusName,
                  Department = joined.Department.DeptName,
                  Location = joined.Location.LocationName,

              });

            var queryList = query.ToList();

            // Apply Sorting
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                string orderByString = $"{sortField} {sortOrder}";
                queryList = queryList.AsQueryable().OrderBy(orderByString).ToList();
            }

            // Convert dates to string after sorting
            var finalQueryList = queryList.Select(q => new ResolvedTicketDto
            {
                Id = q.Id,
                TicketName = q.TicketName,
                EmployeeName = q.EmployeeName,
                Location = q.Location,
                Department = q.Department,
                SubmittedDate = q.SubmittedDate.ToString("yyyy-MM-dd hh:mm tt"),
                ResolvedDate = q.ResolvedDate.ToString("yyyy-MM-dd hh:mm tt"),
                Priority = q.Priority,
                Status = q.Status,
            }).ToList();

            // Apply Pagination
            var totalCount = finalQueryList.Count();
            finalQueryList = finalQueryList.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            // Return the paginated and sorted resolved ticket data along with the total count
            return new AgentTicketResponse<ResolvedTicketDto>
            {
                Data = finalQueryList,
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
                    PriorityName = _context.TBL_PRIORITY
                                    .Where(priority => priority.Id == _context.TBL_TICKET
                                        .Where(ticket => ticket.Id == tracking.TicketId)
                                        .Select(ticket => ticket.PriorityId)
                                        .FirstOrDefault())
                                    .Select(priority => priority.PriorityName)
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
                })
                .ToList();

            return result;
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

            return " ";
        }
    }
}