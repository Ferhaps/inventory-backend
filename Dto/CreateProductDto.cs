
namespace InventorizationBackend.Dto
{
  public class CreateProductDto
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public int CategoryId { get; set; }

  }
}
