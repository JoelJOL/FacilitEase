using FacilitEase.Models.ApiModels;
using FacilitEase.UnitOfWork;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Services
{
    public class DepartmentService:IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly object employeeDto;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<DepartmentDto> GetDepartments()
        {

            var departments = _unitOfWork.Departments.GetAll();
            return MapToDepartmentDtoList(departments);
        }

     
        private IEnumerable<DepartmentDto> MapToDepartmentDtoList(IEnumerable<TBL_DEPARTMENT> departments)
        {
            return departments.Select(MapToDepartmentDto);
        }

        private DepartmentDto MapToDepartmentDto(TBL_DEPARTMENT departments)
        {
            return new DepartmentDto
            {
                Id = departments.Id,
                DeptName = departments.DeptName,
                CreatedBy = departments.CreatedBy,
                CreatedDate = departments.CreatedDate,
                UpdatedBy = departments.UpdatedBy,
                UpdatedDate = departments.UpdatedDate,
            };
        }
        public void CreateDepartment(DepartmentDto departmentDto)
        {
            
            var departmentEntity = MapToTBL_DEPARTMENT(departmentDto);
            _unitOfWork.Departments.Add(departmentEntity);
            _unitOfWork.Complete();
        }

        private TBL_DEPARTMENT MapToTBL_DEPARTMENT(DepartmentDto departmentDto)
        {
            
            return new TBL_DEPARTMENT
            {
                Id = departmentDto.Id,
                DeptName = departmentDto.DeptName,
                CreatedBy = departmentDto.CreatedBy,
                CreatedDate = departmentDto.CreatedDate,
                UpdatedBy = departmentDto.UpdatedBy,
                UpdatedDate = departmentDto.UpdatedDate,
            };
        }

        public IEnumerable<TBL_DEPARTMENT> GetAllDepartments()
        {

            var dept = _unitOfWork.Department.GetAll();
            return (dept);
        }
    }
}
