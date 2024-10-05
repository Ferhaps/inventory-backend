using AuthDemo.Data;
using InventorizationBackend.Dto;
using InventorizationBackend.Interfaces;
using InventorizationBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace InventorizationBackend.Services
{
  public class ProductService(DataContext context) : IProductService
  {
    private readonly DataContext _context = context;

    public async Task<ICollection<Product>> GetProductsAsync()
    {
      return await _context.Products.OrderBy(p => p.Id).ToListAsync();
    }

    public async Task<ProductDto> CreateProductAsync(int categoryId, string productName)
    {
      var category = await _context.Categories.FindAsync(categoryId);
      if (category == null)
      {
        throw new ArgumentException("Invalid category ID", nameof(categoryId));
      }

      var product = new Product
      {
        Name = productName,
        CategoryId = categoryId,
        Quantity = 0
      };

      _context.Products.Add(product);
      await _context.SaveChangesAsync();

      return new ProductDto
      {
          Id = product.Id,
          CategoryName = category.Name,
          Quantity = 0,
          Name = productName
      };
    }
  }
}
