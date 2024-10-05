using AuthDemo.Data;
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

    public async Task<Product> CreateProductAsync(Product product)
    {
      _context.Products.Add(product);
      await _context.SaveChangesAsync();
      return product;
    }
  }
}
