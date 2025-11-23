using Microsoft.AspNetCore.Mvc;
using ReviewService.Application.Common;
using ReviewService.Application.Features.Comments.Command.AddReaction;
using ReviewService.Application.Features.Comments.Command.CreateComment;
using ReviewService.Application.Features.Comments.Command.DeleteComment;
using ReviewService.Application.Features.Comments.Command.UpdateComment;
using ReviewService.Application.Features.Comments.DTOs;
using ReviewService.Application.Features.Comments.Queries.GetCommentById;
using ReviewService.Application.Features.Comments.Queries.GetCommentReplies;
using ReviewService.Application.Features.Comments.Queries.GetCommentsByDiscussion;
using ReviewService.Application.Features.Comments.Queries.GetCommentStats;

namespace ReviewService.API.Controllers
{
    public class CommentsController : ApiControllerBase
    {
        [HttpGet("discussion/{discussionId}")]
        public async Task<ActionResult<PagedList<CommentDto>>> GetCommentsByDiscussion(
            string discussionId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] bool includeReplies = false)
        {
            var query = new GetCommentsByDiscussionQuery(discussionId, page, pageSize, includeReplies);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CommentDto>> GetComment(string id)
        {
            var query = new GetCommentByIdQuery(id);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("{id}/replies")]
        public async Task<ActionResult<PagedList<CommentDto>>> GetCommentReplies(
            string id,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var query = new GetCommentRepliesQuery(id, page, pageSize);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateComment(CreateCommentCommand command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateComment(string id, UpdateCommentCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComment(string id)
        {
            var command = new DeleteCommentCommand(id);
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPost("{id}/like")]
        public async Task<ActionResult> LikeComment(string id)
        {
            var command = new AddReactionCommand(id, true);
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPost("{id}/dislike")]
        public async Task<ActionResult> DislikeComment(string id)
        {
            var command = new AddReactionCommand(id, false);
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpGet("discussion/{discussionId}/stats")]
        public async Task<ActionResult<CommentStatsDto>> GetCommentStats(string discussionId)
        {
            var query = new GetCommentStatsQuery(discussionId);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }
    }

}
