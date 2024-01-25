using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Repositories
{
    public class EmployeeRepository : Repository<TBL_EMPLOYEE>, IEmployeeRepository, Repository<ManagerSubordinateEmployee>
    {
        public EmployeeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
