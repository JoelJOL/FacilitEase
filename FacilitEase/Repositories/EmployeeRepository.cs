// EmployeeRepository.cs
using FacilitEase.Data;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Repositories
{
    public class EmployeeRepository : Repository<TBL_EMPLOYEE>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context) : base(context)
        {
        }

        // Additional methods specific to EmployeeRepository if needed
    }
}
