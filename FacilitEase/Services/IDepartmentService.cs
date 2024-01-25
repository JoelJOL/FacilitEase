using FacilitEase.Models.EntityModels;

namespace FacilitEase.Services
{
    public interface IDepartmentService
    {
        public IEnumerable<TBL_DEPARTMENT> GetAllDepartments();
    }
}
