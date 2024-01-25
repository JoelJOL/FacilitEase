using FacilitEase.Models.ApiModels;
using FacilitEase.UnitOfWork;
using FacilitEase.Models.EntityModels;

namespace FacilitEase.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                CategoryName= categoryDto.CategoryName,
                DepartmentId = categoryDto.DepartmentId,
                CreatedBy = categoryDto.CreatedBy,
                CreatedDate = categoryDto.CreatedDate,
                UpdatedBy = categoryDto.UpdatedBy,
                UpdatedDate = categoryDto.UpdatedDate,
            };
        }

    }
}
