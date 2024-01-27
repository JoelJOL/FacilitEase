using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using FacilitEase.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace FacilitEase.Services
{
    public class L3AdminService:IL3AdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public L3AdminService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public IEnumerable<TBL_TICKET> GetAllTickets()
        {


            var ticket = _unitOfWork.Ticket.GetAll();
            return (ticket);
        }
        public TBL_TICKET GetTicketById(int id)
        {
            var ticket = _unitOfWork.Ticket.GetById(id);
            return (ticket);
        }

        public void CloseTicket(int ticketId)
        {
            var ticketToClose = _context.TBL_TICKET
                .FirstOrDefault(t => t.Id == ticketId);

            if (ticketToClose != null)
            {
                ticketToClose.StatusId = 3;
                _context.SaveChanges();
            }

        }

        public void ForwardTicketToDept(int ticketId, int deptId)
        {
            var l2AdminEmployeeId = _context.TBL_USER_ROLE_MAPPING
            .Where(mapping => mapping.UserRoleId == GetL2AdminRoleId() && mapping.UserId != null)
            .Join(_context.TBL_USER,
            mapping => mapping.UserId,
            user => user.Id,
            (mapping, user) => new { UserId = user.Id, user.EmployeeId })
            .Join(_context.TBL_EMPLOYEE_DETAIL,
             user => user.UserId,
                employeeDetail => employeeDetail.EmployeeId,
                (user, employeeDetail) => new { user.EmployeeId, employeeDetail.DepartmentId })
                .Where(result => result.DepartmentId == deptId)
                .Select(result => result.EmployeeId)
                .FirstOrDefault();

            var ticketToUpdate = _context.TBL_TICKET
               .FirstOrDefault(t => t.Id == ticketId);

            if (ticketToUpdate != null)
            {
                ticketToUpdate.StatusId = 1;
                //Need for fix
               /* ticketToUpdate.DepartmentId = deptId;*/
                _context.SaveChanges();
            }

        }

        private int GetL2AdminRoleId()
        {

            return _context.TBL_USER_ROLE
                .Where(role => role.UserRoleName.ToLower() == "l2admin")
                .Select(role => role.Id)
                .FirstOrDefault();
        }



        public void ForwardTicket(int ticketId, int managerId)
        {
            var ticketToForward = _context.TBL_TICKET
                .FirstOrDefault(t => t.Id == ticketId);
            var manager = _context.TBL_EMPLOYEE.FirstOrDefault(e => e.Id == managerId);
            if (ticketToForward != null)
            {
                ticketToForward.StatusId = 5;

                if (manager?.ManagerId != null)
                {
                    ticketToForward.ControllerId = managerId;
                    _context.SaveChanges();
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

   
        public IEnumerable<TicketJoin> GetTicketDetailsByAgent(int agentId)
        {
            var query = from ticket in _context.TBL_TICKET
                        join user in _context.TBL_USER on ticket.UserId equals user.Id
                        join employee in _context.TBL_EMPLOYEE on user.EmployeeId equals employee.Id
                        join category in _context.TBL_CATEGORY on ticket.CategoryId equals category.Id
                        join status in _context.TBL_STATUS on ticket.StatusId equals status.Id
                        join priority in _context.TBL_PRIORITY on ticket.PriorityId equals priority.Id
                        where ticket.AssignedTo == agentId & status.StatusName == "In Progress"
                        select new TicketJoin
                        {
                            Id = ticket.Id,
                            TicketName = ticket.TicketName,
                            PriorityName = priority.PriorityName,
                            StatusName = status.StatusName,

                            SubmittedDate = ticket.SubmittedDate,
                            EmployeeName = $"{employee.FirstName} {employee.LastName}"
                        };

            return query.ToList();
        }
         public IEnumerable<TicketJoin> GetLatestTicketsByAgent(int agentId)
        {
            try
            {
                var query = from ticket in _context.TBL_TICKET
                            join user in _context.TBL_USER on ticket.UserId equals user.Id
                            join employee in _context.TBL_EMPLOYEE on user.EmployeeId equals employee.Id
                            join category in _context.TBL_CATEGORY on ticket.CategoryId equals category.Id
                            join status in _context.TBL_STATUS on ticket.StatusId equals status.Id
                            join priority in _context.TBL_PRIORITY on ticket.PriorityId equals priority.Id
                            where ticket.AssignedTo == agentId & status.StatusName == "In Progress"
                            orderby ticket.SubmittedDate descending
                            select new TicketJoin
                            {
                                Id = ticket.Id,
                                TicketName = ticket.TicketName,
                                PriorityName = priority.PriorityName,
                                StatusName = status.StatusName,
                                SubmittedDate = ticket.SubmittedDate,
                                EmployeeName = $"{employee.FirstName} {employee.LastName}",
                            };

       
                var results = query.ToList();
                return results;
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw; // Rethrow the exception for proper error handling
            }
        }

        public IEnumerable<Join> GetTicketDetailByAgent(int desiredTicketId)
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
                              RaisedEmployeeName = $"{employee.FirstName} {employee.LastName}",
                              ManagerName = manager != null ? $"{manager.FirstName} {manager.LastName}" : null,
                              ManagerId= manager.ManagerId,
                              LocationName = location.LocationName,
                              DeptName = department.DeptName,
                              DocumentLink = document.DocumentLink,
                              ProjectCode = projectcode.ProjectCode
                          }).FirstOrDefault();

            yield return result;
        }

        public IEnumerable<TicketJoin> GetResolvedTicketsByAgent(int agentId)
        {
            var query = from ticket in _context.TBL_TICKET
                        join user in _context.TBL_USER on ticket.UserId equals user.Id
                        join employee in _context.TBL_EMPLOYEE on user.EmployeeId equals employee.Id
                        join category in _context.TBL_CATEGORY on ticket.CategoryId equals category.Id
                        join status in _context.TBL_STATUS on ticket.StatusId equals status.Id
                        join priority in _context.TBL_PRIORITY on ticket.PriorityId equals priority.Id
                        where ticket.AssignedTo == agentId & (status.StatusName == "Resolved")
                        select new TicketJoin
                        {
                            Id = ticket.Id,
                            TicketName = ticket.TicketName,
                            PriorityName = priority.PriorityName,
                            StatusName = status.StatusName,
                            SubmittedDate = ticket.SubmittedDate,
                            EmployeeName = $"{employee.FirstName} {employee.LastName}",
                        };

            var results = query.ToList();
            return results;
        }

    }
}



