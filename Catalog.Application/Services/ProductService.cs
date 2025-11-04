using AutoMapper;
using Catalog.Application.DTO.Product;
using Catalog.Application.Exceptions;
using Catalog.Application.Pagination;
using Catalog.Application.Services.Interfaces;
using Catalog.Application.Specifications;
using Catalog.Domain.Enteties;
using Catalog.Infrastructure.UnitOfWork;

namespace Catalog.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductDto> GetProductByIdAsync(Guid id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                throw new NotFoundException(nameof(Product), id);

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> GetProductWithDetailsAsync(Guid id)
        {
            var product = await _unitOfWork.Products.GetProductWithDetailsAsync(id);
            if (product == null)
                throw new NotFoundException(nameof(Product), id);

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<PagedResult<ProductDto>> GetProductsAsync(PaginationParams pagination)
        {
            return await SearchProductsByNameAsync("", pagination);
        }

        public async Task<PagedResult<ProductDto>> SearchProductsByNameAsync(string searchTerm, PaginationParams pagination)
        {
            var spec = new ProductByNameSearchSpec(searchTerm, (pagination.PageNumber - 1) * pagination.PageSize, pagination.PageSize);

            var products = await _unitOfWork.Products.ListAsync(spec);
            var totalCount = await _unitOfWork.Products.CountAsync(spec);

            return new PagedResult<ProductDto>
            {
                Items = _mapper.Map<IReadOnlyList<ProductDto>>(products),
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

        public async Task<PagedResult<ProductDto>> GetProductsByCategoryAsync(Guid categoryId, PaginationParams pagination)
        {
            var spec = new ProductsByCategoryIdSpec(categoryId, (pagination.PageNumber - 1) * pagination.PageSize, pagination.PageSize);

            var products = await _unitOfWork.Products.ListAsync(spec);
            var totalCount = await _unitOfWork.Products.CountAsync(spec);

            return new PagedResult<ProductDto>
            {
                Items = _mapper.Map<IReadOnlyList<ProductDto>>(products),
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
        {
            var product = _mapper.Map<Product>(createProductDto);
            var createdProduct = await _unitOfWork.Products.AddAsync(product);
            return _mapper.Map<ProductDto>(createdProduct);
        }

        public async Task<ProductDto> UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            var existingProduct = await _unitOfWork.Products.GetByIdAsync(updateProductDto.Id);
            if (existingProduct == null)
                throw new NotFoundException(nameof(Product), updateProductDto.Id);

            _mapper.Map(updateProductDto, existingProduct);
            await _unitOfWork.Products.UpdateAsync(existingProduct);

            return _mapper.Map<ProductDto>(existingProduct);
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                throw new NotFoundException(nameof(Product), id);

            await _unitOfWork.Products.DeleteAsync(product);
        }
    }
}