using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Contracts.ServiceContracts
{
    public interface IDepartmentService
    {
        IEnumerable<DepartmentDto> GetDepartments();

        void CreateDepartment(DepartmentDto departmentDto);

        public IEnumerable<TBL_DEPARTMENT> GetAllDepartmentsExceptUserDepartment(int userId);

        public List<DeptCategoryDto> GetCategoriesByDepartmentId(int departmentId);
    }
}