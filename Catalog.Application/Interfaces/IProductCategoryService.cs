using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.Application.DTO.ProductCategory;

namespace Catalog.Application.Interfaces
{
    public interface IProductCategoryService
    {
        Task<ProductCategoryDto> GetByIdAsync(Guid productId, Guid categoryId);
        Task<bool> ExistsAsync(Guid productId, Guid categoryId);
        Task AddProductToCategoryAsync(ProductCategoryDto productCategoryDto);
        Task RemoveProductFromCategoryAsync(ProductCategoryDto productCategoryDto);
        Task<IReadOnlyList<ProductCategoryDto>> GetByProductIdAsync(Guid productId);
        Task<IReadOnlyList<ProductCategoryDto>> GetByCategoryIdAsync(Guid categoryId);
    }
}
