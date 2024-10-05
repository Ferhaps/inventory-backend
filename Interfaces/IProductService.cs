using InventorizationBackend.Dto;
using InventorizationBackend.Models;
using System.Threading.Tasks;

namespace InventorizationBackend.Interfaces
{
  public interface IProductService
  {
    Task<ICollection<Product>> GetProductsAsync();

    Task<ProductDto> CreateProductAsync(int categoryId, string productName);

    Task<bool> UpdateProductQuantityAsync(int id, int quantity);

    Task<bool> DeleteProductAsync(int productId);
  }
}
