using InventorizationBackend.Models;

namespace InventorizationBackend.Interfaces
{
  public interface ICategoryService
  {
    Task<ICollection<Category>> GetCategoriesAsync();

    Task<Category> GetCategoryAsync(int categoryId);

    Task<Category> CreateCategoryAsync(string categoryName);

    Task<bool> DeleteCategoryAsync(int categoryId);
  }
}
