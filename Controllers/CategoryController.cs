using InventorizationBackend.Interfaces;
using InventorizationBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventorizationBackend.Controllers
{
  [Route("api/categories")]
  [ApiController]
  public class CategoryController(ICategoryService categoryService) : ControllerBase
  {
    private readonly ICategoryService _categoryService = categoryService;

    [HttpPost]
    [Authorize]
    [ProducesResponseType(200, Type = typeof(Category))]
    public async Task<IActionResult> CreateCategoryAsync([FromQuery] string categoryName)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }
      var category = await _categoryService.CreateCategoryAsync(categoryName);

      if (category == null)
      {
        ModelState.AddModelError("", "Something went wrong while saving");
        return StatusCode(500, ModelState);
      }

      return Ok(category);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
      var result = await _categoryService.DeleteCategoryAsync(id);
      if (!result)
      {
        return NotFound();
      }

      return Ok();
    }
  }
}
