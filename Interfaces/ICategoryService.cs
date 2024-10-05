using InventorizationBackend.Models;

namespace InventorizationBackend.Interfaces
{
  public interface ICategoryService
  {
    Task<ICollection<Category>> GetCategoriesAsync();

    Task<Category> GetCategoryAsync(int categoryId);

    bool CreateCategory(Category category);

    bool Save();
  }
}
