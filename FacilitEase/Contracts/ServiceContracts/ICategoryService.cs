using FacilitEase.Models.ApiModels;

namespace FacilitEase.Contracts.ServiceContracts
{
    public interface ICategoryService

    {
        void CreateCategory(CategoryDto categoryDto);
        IEnumerable<CategoryDto> GetCategoryByDepartmentId(int departmentId);

        List<CategoryDto> GetCategoriesForFacilitiease();
    }
}