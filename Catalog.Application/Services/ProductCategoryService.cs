using AutoMapper;
using Catalog.Application.DTO.ProductCategory;
using Catalog.Application.Exceptions;
using Catalog.Application.Interfaces;
using Catalog.Application.Specifications;
using Catalog.Domain.Enteties;
using Catalog.Infrastructure.Specification;
using Catalog.Infrastructure.UnitOfWork;

namespace Catalog.Application.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductCategoryDto> GetByIdAsync(Guid productId, Guid categoryId)
        {
            var productCategory = await _unitOfWork.ProductCategories.GetEntityWithSpecAsync(
                new ProductCategoryByIdSpec(productId, categoryId)
            );

            if (productCategory == null)
                throw new NotFoundException($"ProductCategory relation", $"Product: {productId}, Category: {categoryId}");

            return _mapper.Map<ProductCategoryDto>(productCategory);
        }

        public async Task<bool> ExistsAsync(Guid productId, Guid categoryId)
        {
            return await _unitOfWork.ProductCategories.ExistsAsync(productId, categoryId);
        }

        public async Task AddProductToCategoryAsync(ProductCategoryDto productCategoryDto)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productCategoryDto.ProductId);
            if (product == null)
                throw new NotFoundException(nameof(Product), productCategoryDto.ProductId);

            var category = await _unitOfWork.Categories.GetByIdAsync(productCategoryDto.CategoryId);
            if (category == null)
                throw new NotFoundException(nameof(Category), productCategoryDto.CategoryId);

            if (await _unitOfWork.ProductCategories.ExistsAsync(productCategoryDto.ProductId, productCategoryDto.CategoryId))
                throw new InvalidOperationException($"Product is already in category");

            var productCategory = _mapper.Map<ProductCategory>(productCategoryDto);
            await _unitOfWork.ProductCategories.AddAsync(productCategory);
        }

        public async Task RemoveProductFromCategoryAsync(ProductCategoryDto productCategoryDto)
        {
            var productCategory = await _unitOfWork.ProductCategories.GetEntityWithSpecAsync(
                new ProductCategoryByIdSpec(productCategoryDto.ProductId, productCategoryDto.CategoryId)
            );

            if (productCategory == null)
                throw new NotFoundException($"ProductCategory relation", $"Product: {productCategoryDto.ProductId}, Category: {productCategoryDto.CategoryId}");

            await _unitOfWork.ProductCategories.DeleteAsync(productCategory);
        }

        public async Task<IReadOnlyList<ProductCategoryDto>> GetByProductIdAsync(Guid productId)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product == null)
                throw new NotFoundException(nameof(Product), productId);

            var productCategories = await _unitOfWork.ProductCategories.ListAsync(
                new ProductCategoryByProductIdSpec(productId)
            );

            return _mapper.Map<IReadOnlyList<ProductCategoryDto>>(productCategories);
        }

        public async Task<IReadOnlyList<ProductCategoryDto>> GetByCategoryIdAsync(Guid categoryId)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);
            if (category == null)
                throw new NotFoundException(nameof(Category), categoryId);

            var productCategories = await _unitOfWork.ProductCategories.ListAsync(
                new ProductCategoryByCategoryIdSpec(categoryId)
            );

            return _mapper.Map<IReadOnlyList<ProductCategoryDto>>(productCategories);
        }
    }
}