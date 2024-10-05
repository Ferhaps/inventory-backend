using AutoMapper;
using InventorizationBackend.Dto;
using InventorizationBackend.Models;

namespace InventorizationBackend.Helper
{
  public class MappingProfiles : Profile
  {
    public MappingProfiles()
    {
      CreateMap<Product, ProductDto>()
        .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
    }
  }
}
