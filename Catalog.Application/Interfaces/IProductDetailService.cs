using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.Application.DTO.ProductDetail;

namespace Catalog.Application.Interfaces
{
    public interface IProductDetailService
    {
        Task<ProductDetailDto> GetProductDetailByIdAsync(Guid id);
        Task<ProductDetailDto> GetByProductIdAsync(Guid productId);
        Task<ProductDetailDto> CreateProductDetailAsync(ProductDetailDto productDetailDto);
        Task<ProductDetailDto> UpdateProductDetailAsync(ProductDetailDto productDetailDto);
        Task DeleteProductDetailAsync(Guid productDetailId);
    }
}
