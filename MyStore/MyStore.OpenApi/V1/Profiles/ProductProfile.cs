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
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<Product, ProductViewModel>();
        }
    }
}