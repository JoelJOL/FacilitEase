using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Services
{
    public interface IDepartmentService
    {
        IEnumerable<DepartmentDto> GetDepartments();

        void CreateDepartment(DepartmentDto departmentDto);

        public IEnumerable<TBL_DEPARTMENT> GetAllDepartments();
    }
}