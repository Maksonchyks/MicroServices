using Microsoft.AspNetCore.Mvc;
using ReviewService.Application.Common;
using ReviewService.Application.Features.Discussions.Commands.CloseDiscussion;
using ReviewService.Application.Features.Discussions.Commands.CreateDiscussion;
using ReviewService.Application.Features.Discussions.Commands.DeleteDiscussion;
using ReviewService.Application.Features.Discussions.Commands.IncrementDiscussionViewCount;
using ReviewService.Application.Features.Discussions.Commands.PinDiscussion;
using ReviewService.Application.Features.Discussions.Commands.UnpinDiscussion;
using ReviewService.Application.Features.Discussions.Commands.UpdateDiscussion;
using ReviewService.Application.Features.Discussions.DTOs;
using ReviewService.Application.Features.Discussions.Queries.GetById;
using ReviewService.Application.Features.Discussions.Queries.GetList;

namespace ReviewService.API.Controllers
{
    public class DiscussionsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PagedList<DiscussionDto>>> GetDiscussions(
            [FromQuery] string? category = null,
            [FromQuery] string? searchTerm = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var query = new GetDiscussionsListQuery(category, searchTerm, page, pageSize);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DiscussionDto>> GetDiscussion(string id)
        {
            var query = new GetDiscussionByIdQuery(id);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateDiscussion(CreateDiscussionCommand command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<string>> UpdateDiscussion(string id, UpdateDiscussionCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDiscussion(string id)
        {
            var command = new DeleteDiscussionCommand(id);
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPost("{id}/pin")]
        public async Task<ActionResult> PinDiscussion(string id)
        {
            var command = new PinDiscussionCommand(id);
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPost("{id}/unpin")]
        public async Task<ActionResult> UnpinDiscussion(string id)
        {
            var command = new UnpinDiscussionCommand(id);
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPost("{id}/close")]
        public async Task<ActionResult> CloseDiscussion(string id)
        {
            var command = new CloseDiscussionCommand(id);
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPost("{id}/increment-views")]
        public async Task<ActionResult> IncrementViewCount(string id)
        {
            var command = new IncrementDiscussionViewCountCommand(id);
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }
    }

}
