using AutoMapper;
using InventorizationBackend.Dto;
using InventorizationBackend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventorizationBackend.Controllers
{
  [Route("api/categories")]
  [ApiController]
  public class CategoryController(ICategoryService categoryService, IMapper mapper) : ControllerBase
  {
    private readonly ICategoryService _categoryService = categoryService;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    [Authorize]
    [ProducesResponseType(200, Type = typeof(ICollection<CategoryDto>))]
    public async Task<IActionResult> GetCategories()
    {
      var categories = await _categoryService.GetCategoriesAsync();
      var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);
      return Ok(categoryDtos);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(200, Type = typeof(CategoryDto))]
    public async Task<IActionResult> CreateCategory([FromQuery] string categoryName)
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

      var categoryDto = _mapper.Map<CategoryDto>(category);
      return Ok(categoryDto);
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
