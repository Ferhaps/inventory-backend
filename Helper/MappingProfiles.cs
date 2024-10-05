using AutoMapper;
using InventorizationBackend.Dto;
using InventorizationBackend.Models;

namespace InventorizationBackend.Helper
{
  public class MappingProfiles : Profile
  {
    public MappingProfiles()
    {
      CreateMap<Product, GetProductDto>()
        .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

      CreateMap<Product, CreateProductDto>();
    }
  }
}
