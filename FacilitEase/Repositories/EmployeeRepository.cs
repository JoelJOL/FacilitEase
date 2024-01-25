using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Repositories
{
    public class EmployeeRepository : Repository<ManagerSubordinateEmployee>, IEmployeeRepository,Repository<TBL_EMPLOYEE>
    {
        private readonly AppDbContext _context;
        public EmployeeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
