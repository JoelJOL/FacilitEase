using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using FacilitEase.UnitOfWork;

namespace FacilitEase.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public CategoryService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public IEnumerable<CategoryDto> GetCategory()
        {
            var category = _unitOfWork.Category.GetAll();
            return MapToCategoryDtoList(category);
        }

        private IEnumerable<CategoryDto> MapToCategoryDtoList(IEnumerable<TBL_CATEGORY> departments)
        {
            return departments.Select(MapToCategoryDto);
        }

        private CategoryDto MapToCategoryDto(TBL_CATEGORY category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                DepartmentId = category.DepartmentId,
                CreatedBy = category.CreatedBy,
                CreatedDate = category.CreatedDate,
                UpdatedBy = category.UpdatedBy,
                UpdatedDate = category.UpdatedDate,
            };
        }

        /// <summary>
        /// To get the categories of particular department
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public IEnumerable<CategoryDto> GetCategoryByDepartmentId(int departmentId)
        {
            var categories = _context.TBL_CATEGORY
                .Where(category => category.DepartmentId == departmentId)
                .ToList();
            var categoryDtos = categories.Select(category => new CategoryDto
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
            });

            return categoryDtos;
        }

        public void CreateCategory(CategoryDto categoryDto)
        {
            var categoryEntity = MapToTBL_DEPARTMENT(categoryDto);
            _unitOfWork.Category.Add(categoryEntity);
            _unitOfWork.Complete();
        }

        private TBL_CATEGORY MapToTBL_DEPARTMENT(CategoryDto categoryDto)
        {
            return new TBL_CATEGORY
            {
                Id = categoryDto.Id,
                CategoryName = categoryDto.CategoryName,
                DepartmentId = categoryDto.DepartmentId,
                CreatedBy = categoryDto.CreatedBy,
                CreatedDate = categoryDto.CreatedDate,
                UpdatedBy = categoryDto.UpdatedBy,
                UpdatedDate = categoryDto.UpdatedDate,
            };
        }
    }
}