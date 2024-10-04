using InventorizationBackend.Models;

namespace InventorizationBackend.Dto
{
  public class ProductDto
  {
    public int Id { get; set; }
    public string Name { get; set; }

    public string CategoryName { get; set; }
  }
}
