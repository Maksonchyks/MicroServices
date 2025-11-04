using AutoMapper;
using Catalog.Application.DTO.ProductDetail;
using Catalog.Application.Exceptions;
using Catalog.Application.Interfaces;
using Catalog.Domain.Enteties;
using Catalog.Infrastructure.UnitOfWork;

namespace Catalog.Application.Services
{
    public class ProductDetailService : IProductDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductDetailService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductDetailDto> GetProductDetailByIdAsync(Guid id)
        {
            var productDetail = await _unitOfWork.ProductDetails.GetByIdAsync(id);
            if (productDetail == null)
                throw new NotFoundException(nameof(ProductDetail), id);

            return _mapper.Map<ProductDetailDto>(productDetail);
        }

        public async Task<ProductDetailDto> GetByProductIdAsync(Guid productId)
        {
            var productDetail = await _unitOfWork.ProductDetails.GetByProductIdAsync(productId);
            if (productDetail == null)
                throw new NotFoundException($"ProductDetail for product", productId);

            return _mapper.Map<ProductDetailDto>(productDetail);
        }

        public async Task<ProductDetailDto> CreateProductDetailAsync(ProductDetailDto productDetailDto)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productDetailDto.ProductId);
            if (product == null)
                throw new NotFoundException(nameof(Product), productDetailDto.ProductId);

            var existingDetail = await _unitOfWork.ProductDetails.GetByProductIdAsync(productDetailDto.ProductId);
            if (existingDetail != null)
                throw new InvalidOperationException($"Product details already exist for product");

            var productDetail = _mapper.Map<ProductDetail>(productDetailDto);
            var createdDetail = await _unitOfWork.ProductDetails.AddAsync(productDetail);
            return _mapper.Map<ProductDetailDto>(createdDetail);
        }

        public async Task<ProductDetailDto> UpdateProductDetailAsync(ProductDetailDto productDetailDto)
        {
            var existingDetail = await _unitOfWork.ProductDetails.GetByIdAsync(productDetailDto.ProductDetailId);
            if (existingDetail == null)
                throw new NotFoundException(nameof(ProductDetail), productDetailDto.ProductDetailId);

            _mapper.Map(productDetailDto, existingDetail);
            await _unitOfWork.ProductDetails.UpdateAsync(existingDetail);

            return _mapper.Map<ProductDetailDto>(existingDetail);
        }

        public async Task DeleteProductDetailAsync(Guid productDetailId)
        {
            var productDetail = await _unitOfWork.ProductDetails.GetByIdAsync(productDetailId);
            if (productDetail == null)
                throw new NotFoundException(nameof(ProductDetail), productDetailId);

            await _unitOfWork.ProductDetails.DeleteAsync(productDetail);
        }
    }
}