using AutoMapper;
using MyStore.OpenApi.Entities;
using MyStore.OpenApi.V1.Controllers;
using MyStore.OpenApi.V1.ViewModels;

namespace MyStore.OpenApi.V1.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<Category, CategoryViewModel>();
        }
    }
}