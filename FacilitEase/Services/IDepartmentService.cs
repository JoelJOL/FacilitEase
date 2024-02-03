using FacilitEase.Models.ApiModels;
using System.Collections.Generic;
﻿using FacilitEase.Models.EntityModels;
namespace FacilitEase.Services
{
    public interface IDepartmentService
    {
        IEnumerable<DepartmentDto> GetDepartments();
        void CreateDepartment(DepartmentDto departmentDto);
        public IEnumerable<Department> GetAllDepartments();
        public List<DeptCategoryDto> GetCategoriesByDepartmentId(int departmentId);
    }
}
