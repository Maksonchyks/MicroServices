using Catalog.Application.DTO.Product;
using Catalog.Application.Pagination;

namespace Catalog.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> GetProductByIdAsync(Guid id);
        Task<ProductDto> GetProductWithDetailsAsync(Guid id);
        Task<PagedResult<ProductDto>> GetProductsAsync(PaginationParams pagination);
        Task<PagedResult<ProductDto>> SearchProductsByNameAsync(string searchTerm, PaginationParams pagination);
        Task<PagedResult<ProductDto>> GetProductsByCategoryAsync(Guid categoryId, PaginationParams pagination);
        Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
        Task<ProductDto> UpdateProductAsync(UpdateProductDto updateProductDto);
        Task DeleteProductAsync(Guid id);
    }
}