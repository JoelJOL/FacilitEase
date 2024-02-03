using FacilitEase.Data;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Repositories
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository

    {
        public DepartmentRepository(AppDbContext context) : base(context) { }
    }

}
