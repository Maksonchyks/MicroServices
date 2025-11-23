using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReviewService.Application.Common;

namespace ReviewService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        private ISender _mediator = null!;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        protected ActionResult<T> HandleResult<T>(Result<T> result)
        {
            if (result == null) return NotFound();
            if (result.IsSuccess && result.Value != null)
                return Ok(result.Value);
            if (result.IsSuccess && result.Value == null)
                return NotFound();
            return BadRequest(result.Error);
        }

        protected ActionResult HandleResult(Result result)
        {
            if (result == null) return NotFound();
            if (result.IsSuccess)
                return Ok();
            return BadRequest(result.Error);
        }
    }
}
