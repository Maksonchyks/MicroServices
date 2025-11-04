using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.Application.DTO.ProductImage;

namespace Catalog.Application.Interfaces
{
    public interface IProductImageService
    {
        Task<ProductImageDto> GetProductImageByIdAsync(Guid id);
        Task<IReadOnlyList<ProductImageDto>> GetAllByProductIdAsync(Guid productId);
        Task<ProductImageDto> AddProductImageAsync(ProductImageDto productImageDto);
        Task DeleteProductImageAsync(Guid productImageId);
        Task DeleteAllProductImagesAsync(Guid productId);
    }
}
