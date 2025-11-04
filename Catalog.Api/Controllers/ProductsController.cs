using Catalog.Application.DTO.Product;
using Catalog.Application.Pagination;
using Catalog.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<ProductDto>>> GetProducts(
            [FromQuery] PaginationParams paginationParams)
        {
            var products = await _productService.GetProductsAsync(paginationParams);
            return Ok(products);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductDto>> GetProduct(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return Ok(product);
        }

        [HttpGet("{id:guid}/details")]
        public async Task<ActionResult<ProductDto>> GetProductWithDetails(Guid id)
        {
            var product = await _productService.GetProductWithDetailsAsync(id);
            return Ok(product);
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagedResult<ProductDto>>> SearchProducts(
            [FromQuery] string searchTerm,
            [FromQuery] PaginationParams paginationParams)
        {
            var products = await _productService.SearchProductsByNameAsync(searchTerm, paginationParams);
            return Ok(products);
        }

        [HttpGet("category/{categoryId:guid}")]
        public async Task<ActionResult<PagedResult<ProductDto>>> GetProductsByCategory(
            Guid categoryId,
            [FromQuery] PaginationParams paginationParams)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId, paginationParams);
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto createProductDto)
        {
            var product = await _productService.CreateProductAsync(createProductDto);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(Guid id, UpdateProductDto updateProductDto)
        {
            if (id != updateProductDto.Id)
                return BadRequest("ID in route doesn't match ID in body");

            var product = await _productService.UpdateProductAsync(updateProductDto);
            return Ok(product);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
