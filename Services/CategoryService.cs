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
      return await _context.Categories.OrderBy(c => c.Id).ToListAsync();
    }

    public async Task<Category> GetCategoryAsync(int categoryId)
    {
      return await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
    }

    public async Task<Category> CreateCategoryAsync(string name)
    {
      var category = new Category { Name = name, Products = new List<Product>() };
      _context.Categories.Add(category);
      await _context.SaveChangesAsync();
      return category;
    }
  }
}
