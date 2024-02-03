using FacilitEase.Data;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Repositories
{
    public class EmployeeDetailRepository : Repository<TBL_EMPLOYEE_DETAIL>, IEmployeeDetailRepository
    {
        public EmployeeDetailRepository(AppDbContext context) : base(context)
        {
        }

        public void AddRange(IEnumerable<TBL_EMPLOYEE_DETAIL> employeeDetails)
        {
            // This implementation will call the AddRange method from the base class (generic repository)
            base.AddRange(employeeDetails);
        }
    }
}