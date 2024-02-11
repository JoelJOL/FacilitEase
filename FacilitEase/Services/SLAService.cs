using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.Repositories;
using FacilitEase.UnitOfWork;

namespace FacilitEase.Services
{
    public class SLAService : ISLAService
    {
        private readonly AppDbContext _context;
        public SLAService(AppDbContext context)
        {

            _context = context;
        }
        
        public void EditSLA(int departmentId, int priorityId, int Time)
        {
            var selectedSla = (from sla in _context.TBL_SLA
                              join department in _context.TBL_DEPARTMENT on sla.DepartmentId equals department.Id
                              join priority in _context.TBL_PRIORITY on sla.PriorityId equals priority.Id
                              where ((sla.DepartmentId == departmentId) && (sla.PriorityId == priorityId))
                              select sla).FirstOrDefault();
            if (selectedSla != null)
            {
                selectedSla.Time = Time;
            }
            _context.SaveChanges();

        }

        public List<SLAInfo> GetSLAInfo(int userid)
        {
            var slaInfo = from user in _context.TBL_USER
                           join employee in _context.TBL_EMPLOYEE on user.EmployeeId equals employee.Id
                           join employeedetail in _context.TBL_EMPLOYEE_DETAIL on employee.Id equals employeedetail.EmployeeId
                           join department in _context.TBL_DEPARTMENT on employeedetail.DepartmentId equals department.Id
                           join sla in _context.TBL_SLA on department.Id equals sla.DepartmentId
                            where user.Id == userid
                           select new SLAInfo
                            {
                                DepartmentId = sla.DepartmentId,
                                PriorityId = sla.PriorityId,
                                Time = sla.Time

                            };
            var slaInfoList = slaInfo.ToList();
            return slaInfoList;

        }
    }
}
