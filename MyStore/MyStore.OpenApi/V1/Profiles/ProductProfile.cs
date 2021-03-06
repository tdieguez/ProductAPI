using AutoMapper;
using MyStore.OpenApi.Entities;
using MyStore.OpenApi.V1.Dtos;

namespace MyStore.OpenApi.V1.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductDto>();
        }
    }
}