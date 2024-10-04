using InventorizationBackend.Models;

namespace InventorizationBackend.Interfaces
{
  public interface IProductService
  {
    Task<ICollection<Product>> GetProductsAsync();

    bool Save();
  }
}
