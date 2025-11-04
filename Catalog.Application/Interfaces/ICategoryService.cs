using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.Application.DTO.Category;
using Catalog.Application.Pagination;

namespace Catalog.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDto> GetCategoryByIdAsync(Guid id);
        Task<CategoryDto> GetCategoryWithProductsAsync(Guid id);
        Task<IReadOnlyList<CategoryDto>> GetAllCategoriesAsync();
        Task<PagedResult<CategoryDto>> GetCategoriesWithProductCountAsync(PaginationParams pagination);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto);
        Task<CategoryDto> UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto);
        Task DeleteCategoryAsync(Guid id);
    }
}
