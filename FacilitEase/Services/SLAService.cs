using FacilitEase.Data;
using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public class SLAService : ISLAService
    {
        private readonly AppDbContext _context;

        public SLAService(AppDbContext context)
        {
            _context = context;
        }

        public void EditSLA(int departmentId, int CategoryId, int Time)
        {
            var selectedSla = (from sla in _context.TBL_SLA
                               join department in _context.TBL_DEPARTMENT on sla.DepartmentId equals department.Id
                               join category in _context.TBL_CATEGORY on sla.CategoryId equals category.Id
                               where ((sla.DepartmentId == departmentId) && (sla.CategoryId == CategoryId))
                               select sla).FirstOrDefault();
            if (selectedSla != null)
            {
                selectedSla.Time = Time;
            }
            _context.SaveChanges();
        }

        public List<ShowSLAInfo> GetSLAInfo(int userid)
        {
            var slaInfo = from user in _context.TBL_USER
                          join employee in _context.TBL_EMPLOYEE on user.EmployeeId equals employee.Id
                          join employeedetail in _context.TBL_EMPLOYEE_DETAIL on employee.Id equals employeedetail.EmployeeId
                          join department in _context.TBL_DEPARTMENT on employeedetail.DepartmentId equals department.Id
                          join sla in _context.TBL_SLA on department.Id equals sla.DepartmentId
                          join category in _context.TBL_CATEGORY on sla.CategoryId equals category.Id
                          where user.Id == userid
                          select new ShowSLAInfo
                          {
                              DepartmentId = sla.DepartmentId,
                              CategoryName = category.CategoryName,
                              Time = sla.Time
                          };
            var slaInfoList = slaInfo.ToList();
            return slaInfoList;
        }

        public int GetTicketSLA(int ticketId)
        {
            var selectedTicket = (from ticket in _context.TBL_TICKET
                                  where (ticket.Id == ticketId)
                                  select ticket).FirstOrDefault();

            return selectedTicket.EscalationTime;

        }
        public void EditTicketSLA(int ticketId, int time)
        {
            var selectedTicket = (from ticket in _context.TBL_TICKET
                                  where (ticket.Id == ticketId)
                                  select ticket).FirstOrDefault();
            if (selectedTicket != null)
            {
                selectedTicket.EscalationTime = time;
            }
            _context.SaveChanges();
        }
    }
}