using Catalog.Application.DTO.ProductImage;
using Catalog.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("api/products/{productId:guid}/images")]
    public class ProductImagesController : ControllerBase
    {
        private readonly IProductImageService _productImageService;

        public ProductImagesController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductImageDto>>> GetProductImages(Guid productId)
        {
            var images = await _productImageService.GetAllByProductIdAsync(productId);
            return Ok(images);
        }

        [HttpGet("{imageId:guid}")]
        public async Task<ActionResult<ProductImageDto>> GetProductImage(Guid productId, Guid imageId)
        {
            var image = await _productImageService.GetProductImageByIdAsync(imageId);
            return Ok(image);
        }

        [HttpPost]
        public async Task<ActionResult<ProductImageDto>> AddProductImage(
            Guid productId, ProductImageDto productImageDto)
        {
            if (productId != productImageDto.ProductId)
                return BadRequest("Product ID in route doesn't match Product ID in body");

            var image = await _productImageService.AddProductImageAsync(productImageDto);
            return CreatedAtAction(nameof(GetProductImage),
                new { productId, imageId = image.ProductImageId }, image);
        }

        [HttpDelete("{imageId:guid}")]
        public async Task<IActionResult> DeleteProductImage(Guid productId, Guid imageId)
        {
            await _productImageService.DeleteProductImageAsync(imageId);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllProductImages(Guid productId)
        {
            await _productImageService.DeleteAllProductImagesAsync(productId);
            return NoContent();
        }
    }
}
