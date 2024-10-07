using AuthDemo.Data;
using InventorizationBackend.Interfaces;
using InventorizationBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace InventorizationBackend.Services
{
  public class CategoryService(DataContext context) : ICategoryService
  {
    private readonly DataContext _context = context;

    public async Task<ICollection<Category>> GetCategoriesAsync()
    {
      return await _context.Categories.OrderByDescending(c => c.Id).ToListAsync();
    }

    public async Task<Category> GetCategoryAsync(int categoryId)
    {
      return await _context.Categories.FindAsync(categoryId);
    }

    public async Task<Category> CreateCategoryAsync(string name)
    {
      var category = new Category { Name = name, Products = new List<Product>() };
      _context.Categories.Add(category);
      await _context.SaveChangesAsync();
      return category;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
      var category = await _context.Categories.FindAsync(id);
      if (category == null)
      {
        return false;
      }

      _context.Categories.Remove(category);
      return await _context.SaveChangesAsync() > 0;
    }
  }
}
