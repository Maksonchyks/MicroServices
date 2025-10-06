using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Orders.Bll.Interfaces;

namespace Orders.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private readonly IPromotionService _service;
        public PromotionsController(IPromotionService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var promo = await _service.GetByIdAsync(id);
            return promo == null ? NotFound() : Ok(promo);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PromotionDto dto)
        {
            await _service.AddAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = dto.PromotionId }, dto);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, PromotionDto dto)
        {
            if (id != dto.PromotionId) return BadRequest();
            await _service.UpdateAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
