using System;
using System.Linq.Expressions;
using AutoMapper;
using MyStore.OpenApi.Entities;
using MyStore.OpenApi.V1.Dtos;
using MyStore.OpenApi.V1.ViewModels;

namespace MyStore.OpenApi.V1.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductDto, Product>()
                .ForMember(
                    d => d.Category,
                    o => o.MapFrom(p => new Category() {Id = p.CategoryId})
                )
                .ReverseMap();

            CreateMap<Product, ProductViewModel>();
        }
    }
}