using AutoMapper;
using Catalog.Application.DTO.Category;
using Catalog.Application.Exceptions;
using Catalog.Application.Interfaces;
using Catalog.Application.Pagination;
using Catalog.Application.Specifications;
using Catalog.Domain.Enteties;
using Catalog.Infrastructure.UnitOfWork;

namespace Catalog.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(Guid id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                throw new NotFoundException(nameof(Category), id);

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> GetCategoryWithProductsAsync(Guid id)
        {
            var category = await _unitOfWork.Categories.GetCategoryWithProductsAsync(id);
            if (category == null)
                throw new NotFoundException(nameof(Category), id);

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<IReadOnlyList<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return _mapper.Map<IReadOnlyList<CategoryDto>>(categories);
        }

        public async Task<PagedResult<CategoryDto>> GetCategoriesWithProductCountAsync(PaginationParams pagination)
        {
            var spec = new CategoriesWithProductCountSpec();
            spec.ApplyPaging((pagination.PageNumber - 1) * pagination.PageSize, pagination.PageSize);

            var categories = await _unitOfWork.Categories.ListAsync(spec);
            var totalCount = await _unitOfWork.Categories.CountAsync(spec);

            return new PagedResult<CategoryDto>
            {
                Items = _mapper.Map<IReadOnlyList<CategoryDto>>(categories),
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            var category = _mapper.Map<Category>(createCategoryDto);
            var createdCategory = await _unitOfWork.Categories.AddAsync(category);
            return _mapper.Map<CategoryDto>(createdCategory);
        }

        public async Task<CategoryDto> UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto)
        {
            var existingCategory = await _unitOfWork.Categories.GetByIdAsync(updateCategoryDto.CategoryId);
            if (existingCategory == null)
                throw new NotFoundException(nameof(Category), updateCategoryDto.CategoryId);

            _mapper.Map(updateCategoryDto, existingCategory);
            await _unitOfWork.Categories.UpdateAsync(existingCategory);

            return _mapper.Map<CategoryDto>(existingCategory);
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                throw new NotFoundException(nameof(Category), id);

            var categoryWithProducts = await _unitOfWork.Categories.GetCategoryWithProductsAsync(id);
            if (categoryWithProducts?.ProductCategories?.Count > 0)
                throw new InvalidOperationException($"Cannot delete category with existing products");

            await _unitOfWork.Categories.DeleteAsync(category);
        }
    }
}