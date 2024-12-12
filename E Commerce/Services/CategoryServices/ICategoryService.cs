using E_Commerce.DTOs;

namespace E_Commerce.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetCategories();
        Task<bool> AddCategory(CategoryDto categoryDto);
        Task<bool> RemoveCategory(int id);
    }
}
