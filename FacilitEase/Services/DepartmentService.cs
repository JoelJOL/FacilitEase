using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using FacilitEase.UnitOfWork;

namespace FacilitEase.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly object employeeDto;

        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public DepartmentService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        /// <summary>
        /// To Retrieve all departments from the database and maps them to DepartmentDto objects.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DepartmentDto> GetDepartments()
        {
            var departments = _unitOfWork.Departments.GetAll();
            return MapToDepartmentDtoList(departments);
        }

        /// <summary>
        /// Mapping a collection of TBL_DEPARTMENT entities to a collection of DepartmentDto objects.
        /// </summary>
        /// <param name="departments"></param>
        /// <returns></returns>
        private IEnumerable<DepartmentDto> MapToDepartmentDtoList(IEnumerable<TBL_DEPARTMENT> departments)
        {
            return departments.Select(MapToDepartmentDto);
        }

        /// <summary>
        /// Mapping an individual TBL_DEPARTMENT entity to a DepartmentDto object.
        /// </summary>
        /// <param name="departments"></param>
        /// <returns></returns>
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

        /// <summary>
        /// To add department
        /// </summary>
        /// <param name="departmentDto"></param>
        public void CreateDepartment(DepartmentDto departmentDto)
        {
            var departmentEntity = MapToTBL_DEPARTMENT(departmentDto);
            _unitOfWork.Departments.Add(departmentEntity);
            _unitOfWork.Complete();
        }

        /// <summary>
        /// Mapping a DepartmentDto object to a TBL_DEPARTMENT entity.
        /// </summary>
        /// <param name="departmentDto"></param>
        /// <returns></returns>
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

        /// <summary>
        /// To retrieve all departments from the database using the Unit of Work pattern and returns them as a collection of TBL_DEPARTMENT entities.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TBL_DEPARTMENT> GetAllDepartments()
        {
            var dept = _unitOfWork.Departments.GetAll();
            return (dept);
        }

        public List<DeptCategoryDto> GetCategoriesByDepartmentId(int departmentId)
        {
            return _context.TBL_CATEGORY
                .Where(category => category.DepartmentId == departmentId)
                .Select(category => new DeptCategoryDto
                {
                    Id = category.Id,
                    CategoryName = category.CategoryName
                })
                .ToList();
        }
    }
}