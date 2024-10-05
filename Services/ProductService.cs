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

    public bool CreateProduct(Product product)
    {
      _context.Products.Add(product);
      return Save();
    }

    public bool Save()
    {
      return _context.SaveChanges() > 0;
    }
  }
}
