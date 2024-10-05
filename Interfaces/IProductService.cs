using InventorizationBackend.Dto;
using InventorizationBackend.Models;

namespace InventorizationBackend.Interfaces
{
  public interface IProductService
  {
    Task<ICollection<Product>> GetProductsAsync();

    Task<ProductDto> CreateProductAsync(int categoryId, string productName);
  }
}
