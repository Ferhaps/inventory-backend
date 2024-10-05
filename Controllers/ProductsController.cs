﻿using AutoMapper;
using InventorizationBackend.Dto;
using InventorizationBackend.Interfaces;
using InventorizationBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventorizationBackend.Controllers
{
  [Route("api/products")]
  [ApiController]
  public class ProductsController(IProductService productService, IMapper mapper) : ControllerBase
  {
    private readonly IProductService _productService = productService;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    [Authorize]
    [ProducesResponseType(200, Type = typeof(ICollection<Product>))]
    public async Task<IActionResult> GetProductsAsync()
    {
      var products = await _productService.GetProductsAsync();
      var productDtos = _mapper.Map<List<ProductDto>>(products);

      return Ok(productDtos);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(200, Type = typeof(ProductDto))]
    public async Task<IActionResult> CreateProductAsync([FromQuery] string name, [FromQuery] int categoryId)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        return BadRequest("Product name is required.");
      }

      try
      {
        var product = await _productService.CreateProductAsync(categoryId, name);
        return Ok(product);
      }
      catch (ArgumentException ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpPatch("{id}/quantity")]
    [Authorize]
    public async Task<IActionResult> UpdateProductQuantity(int id, [FromBody] int quantity)
    {
      try
      {
        var success = await _productService.UpdateProductQuantityAsync(id, quantity);
        if (success) { 
          return Ok();
        }

        return BadRequest();
      }
      catch (ArgumentException ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> DeleteProductAsync(int id)
    {
      var result = await _productService.DeleteProductAsync(id);
      if (!result)
      {
        return NotFound();
      }

      return Ok();
    }
  }
}
