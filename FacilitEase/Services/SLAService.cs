using FacilitEase.Data;
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
    }
}
