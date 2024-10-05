using AutoMapper;
using InventorizationBackend.Dto;
using InventorizationBackend.Interfaces;
using InventorizationBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventorizationBackend.Controllers
{
  [Route("api/products")]
  [ApiController]
  public class ProductsController(IProductService productService, ICategoryService categoryService, IMapper mapper) : ControllerBase
  {
    private readonly ICategoryService _categoryService = categoryService;
    private readonly IProductService _productService = productService;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    [Authorize]
    [ProducesResponseType(200, Type = typeof(ICollection<Product>))]
    [ProducesResponseType(401)]
    public async Task<IActionResult> GetProductsAsync()
    {
      var products = await _productService.GetProductsAsync();
      var productDtos = _mapper.Map<List<GetProductDto>>(products);

      return Ok(productDtos);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateProductAsync([FromQuery] int categoryId, [FromBody] CreateProductDto productCreate)
    {
      if (productCreate == null)
      {
        return BadRequest(new { message = "Body is null" });
      }

      var products = await _productService.GetProductsAsync();
      var product = products
        .FirstOrDefault(c => c.Name.Trim().Equals(productCreate.Name.Trim(), StringComparison.CurrentCultureIgnoreCase));

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var productMap = _mapper.Map<Product>(productCreate);
      productMap.Category = await _categoryService.GetCategoryAsync(categoryId);

      if (!_productService.CreateProduct(productMap))
      {
        ModelState.AddModelError("", "Something went wrong while saving");
        return StatusCode(500, ModelState);
      }

      return Ok("Successfully created");
    }
  }
}
