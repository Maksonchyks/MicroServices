using Catalog.Application.DTO.ProductDetail;
using Catalog.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("api/products/{productId:guid}/details")]
    public class ProductDetailsController : ControllerBase
    {
        private readonly IProductDetailService _productDetailService;

        public ProductDetailsController(IProductDetailService productDetailService)
        {
            _productDetailService = productDetailService;
        }

        [HttpGet]
        public async Task<ActionResult<ProductDetailDto>> GetProductDetail(Guid productId)
        {
            var detail = await _productDetailService.GetByProductIdAsync(productId);
            return Ok(detail);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDetailDto>> CreateProductDetail(
            Guid productId, ProductDetailDto productDetailDto)
        {
            if (productId != productDetailDto.ProductId)
                return BadRequest("Product ID in route doesn't match Product ID in body");

            var detail = await _productDetailService.CreateProductDetailAsync(productDetailDto);
            return CreatedAtAction(nameof(GetProductDetail), new { productId }, detail);
        }

        [HttpPut]
        public async Task<ActionResult<ProductDetailDto>> UpdateProductDetail(
            Guid productId, ProductDetailDto productDetailDto)
        {
            if (productId != productDetailDto.ProductId)
                return BadRequest("Product ID in route doesn't match Product ID in body");

            var detail = await _productDetailService.UpdateProductDetailAsync(productDetailDto);
            return Ok(detail);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProductDetail(Guid productId)
        {
            await _productDetailService.DeleteProductDetailAsync(productId);
            return NoContent();
        }
    }
}
