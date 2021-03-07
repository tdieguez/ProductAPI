using AutoMapper;
using MyStore.OpenApi.Entities;
using MyStore.OpenApi.V1.Controllers;

namespace MyStore.OpenApi.V1.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryDto, Category>().ReverseMap();
        }
    }
}