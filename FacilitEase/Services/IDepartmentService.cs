using FacilitEase.Models.ApiModels;
using System.Collections.Generic;

namespace FacilitEase.Services
{
    public interface IDepartmentService
    {
        IEnumerable<DepartmentDto> GetDepartments();
        void CreateDepartment(DepartmentDto departmentDto);
    }
}
