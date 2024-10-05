using InventorizationBackend.Models;

namespace InventorizationBackend.Dto
{
  public class GetProductDto
  {
    public int Id { get; set; }
    public string Name { get; set; }

    public string CategoryName { get; set; }
  }
}
