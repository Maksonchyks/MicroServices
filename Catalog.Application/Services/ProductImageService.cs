using AutoMapper;
using Catalog.Application.DTO.ProductImage;
using Catalog.Application.Exceptions;
using Catalog.Application.Interfaces;
using Catalog.Domain.Enteties;
using Catalog.Infrastructure.UnitOfWork;

namespace Catalog.Application.Services
{
    public class ProductImageService : IProductImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductImageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductImageDto> GetProductImageByIdAsync(Guid id)
        {
            var productImage = await _unitOfWork.ProductImages.GetByIdAsync(id);
            if (productImage == null)
                throw new NotFoundException(nameof(ProductImage), id);

            return _mapper.Map<ProductImageDto>(productImage);
        }

        public async Task<IReadOnlyList<ProductImageDto>> GetAllByProductIdAsync(Guid productId)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product == null)
                throw new NotFoundException(nameof(Product), productId);

            var productImages = await _unitOfWork.ProductImages.GetAllByProductIdAsync(productId);
            return _mapper.Map<IReadOnlyList<ProductImageDto>>(productImages);
        }

        public async Task<ProductImageDto> AddProductImageAsync(ProductImageDto productImageDto)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productImageDto.ProductId);
            if (product == null)
                throw new NotFoundException(nameof(Product), productImageDto.ProductId);

            var productImage = _mapper.Map<ProductImage>(productImageDto);
            var createdImage = await _unitOfWork.ProductImages.AddAsync(productImage);
            return _mapper.Map<ProductImageDto>(createdImage);
        }

        public async Task DeleteProductImageAsync(Guid productImageId)
        {
            var productImage = await _unitOfWork.ProductImages.GetByIdAsync(productImageId);
            if (productImage == null)
                throw new NotFoundException(nameof(ProductImage), productImageId);

            await _unitOfWork.ProductImages.DeleteAsync(productImage);
        }

        public async Task DeleteAllProductImagesAsync(Guid productId)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product == null)
                throw new NotFoundException(nameof(Product), productId);

            var productImages = await _unitOfWork.ProductImages.GetAllByProductIdAsync(productId);
            foreach (var image in productImages)
                await _unitOfWork.ProductImages.DeleteAsync(image);
        }
    }
}