using System.Diagnostics.Metrics;
using AutoMapper;
using WebApplication1.DTOs.Author;
using WebApplication1.DTOs.Category;
using WebApplication1.DTOs.Product;
using WebApplication1.Models;

namespace WebApplication1.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CreateAuthorDto, Author>();
            CreateMap<ProductCreateDto, Product>();
        }
    }
}
