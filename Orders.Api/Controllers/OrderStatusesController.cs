using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Orders.Bll.Interfaces;

namespace Orders.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderStatusesController : ControllerBase
    {
        private readonly IOrderStatusHistoryService _service;
        public OrderStatusesController(IOrderStatusHistoryService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var status = await _service.GetByIdAsync(id);
            return status == null ? NotFound() : Ok(status);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderStatusHistoryDto dto)
        {
            await _service.AddAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = dto.StatusId }, dto);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, OrderStatusHistoryDto dto)
        {
            if (id != dto.StatusId) return BadRequest();
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
