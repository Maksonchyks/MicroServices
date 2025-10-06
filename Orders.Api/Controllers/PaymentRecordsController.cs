using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Orders.Bll.Interfaces;

namespace Orders.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentRecordsController : ControllerBase
    {
        private readonly IPaymentRecordService _service;
        public PaymentRecordsController(IPaymentRecordService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var record = await _service.GetByIdAsync(id);
            return record == null ? NotFound() : Ok(record);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaymentRecordDto dto)
        {
            await _service.AddAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = dto.PaymentId }, dto);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, PaymentRecordDto dto)
        {
            if (id != dto.PaymentId) return BadRequest();
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
