using FacilitEase.Data;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Repositories
{
    public class EmployeeRepository : Repository<TBL_EMPLOYEE>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context) : base(context)
        {
        }

        /*Dont use Repository<ManagerSubordinateEmployee> it is an api model*/

        public void AddRange(IEnumerable<TBL_EMPLOYEE> employees)
        {
            // This implementation will call the AddRange method from the base class (generic repository)
            base.AddRange(employees);
        }
    }
}