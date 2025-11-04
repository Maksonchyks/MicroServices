using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Application.DTO.Category;
using Catalog.Application.DTO.Product;
using Catalog.Application.DTO.ProductCategory;
using Catalog.Application.DTO.ProductDetail;
using Catalog.Application.DTO.ProductImage;
using Catalog.Domain.Enteties;

namespace Catalog.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Product mappings
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
            CreateMap<Product, ProductDto>();

            // Category mappings
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();
            CreateMap<Category, CategoryDto>();

            // Other mappings (без змін)
            CreateMap<ProductDetail, ProductDetailDto>().ReverseMap();
            CreateMap<ProductImage, ProductImageDto>().ReverseMap();
            CreateMap<ProductCategory, ProductCategoryDto>().ReverseMap();
        }
    }
}
