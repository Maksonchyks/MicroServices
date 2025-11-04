using Catalog.Application.DTO.Category;
using Catalog.Application.Interfaces;
using Catalog.Application.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<CategoryDto>>> GetCategoriesPaged(
            [FromQuery] PaginationParams paginationParams)
        {
            var categories = await _categoryService.GetCategoriesWithProductCountAsync(paginationParams);
            return Ok(categories);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(Guid id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            return Ok(category);
        }

        [HttpGet("{id:guid}/with-products")]
        public async Task<ActionResult<CategoryDto>> GetCategoryWithProducts(Guid id)
        {
            var category = await _categoryService.GetCategoryWithProductsAsync(id);
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            var category = await _categoryService.CreateCategoryAsync(createCategoryDto);
            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, category);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(Guid id, UpdateCategoryDto updateCategoryDto)
        {
            if (id != updateCategoryDto.CategoryId)
                return BadRequest("ID in route doesn't match ID in body");

            var category = await _categoryService.UpdateCategoryAsync(updateCategoryDto);
            return Ok(category);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
