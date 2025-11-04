using Catalog.Application.DTO.ProductCategory;
using Catalog.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("api/product-categories")]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoriesController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToCategory(ProductCategoryDto productCategoryDto)
        {
            await _productCategoryService.AddProductToCategoryAsync(productCategoryDto);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveProductFromCategory(ProductCategoryDto productCategoryDto)
        {
            await _productCategoryService.RemoveProductFromCategoryAsync(productCategoryDto);
            return NoContent();
        }

        [HttpGet("product/{productId:guid}")]
        public async Task<ActionResult<IReadOnlyList<ProductCategoryDto>>> GetProductCategories(Guid productId)
        {
            var categories = await _productCategoryService.GetByProductIdAsync(productId);
            return Ok(categories);
        }

        [HttpGet("category/{categoryId:guid}")]
        public async Task<ActionResult<IReadOnlyList<ProductCategoryDto>>> GetCategoryProducts(Guid categoryId)
        {
            var products = await _productCategoryService.GetByCategoryIdAsync(categoryId);
            return Ok(products);
        }
    }
}
