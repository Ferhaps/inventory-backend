using AutoMapper;
using InventorizationBackend.Dto;
using InventorizationBackend.Models;

namespace InventorizationBackend.Helper
{
  public class MappingProfiles : Profile
  {
    public MappingProfiles()
    {
      CreateMap<Product, ProductDto>();
      CreateMap<Category, CategoryDto>();
    }
  }
}
