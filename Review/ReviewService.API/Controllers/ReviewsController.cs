using Microsoft.AspNetCore.Mvc;
using ReviewService.Application.Common;
using ReviewService.Application.Features.Reviews.Commands.AddReviewReaction;
using ReviewService.Application.Features.Reviews.Commands.ApproveReview;
using ReviewService.Application.Features.Reviews.Commands.CreateReview;
using ReviewService.Application.Features.Reviews.Commands.DeleteReview;
using ReviewService.Application.Features.Reviews.Commands.RejectReview;
using ReviewService.Application.Features.Reviews.Commands.UpdateReview;
using ReviewService.Application.Features.Reviews.DTOs;
using ReviewService.Application.Features.Reviews.Queries.GetProductRatingStats;
using ReviewService.Application.Features.Reviews.Queries.GetReviewById;
using ReviewService.Application.Features.Reviews.Queries.GetReviewsByAuthor;
using ReviewService.Application.Features.Reviews.Queries.GetReviewsByProduct;
using ReviewService.Domain.Entities;

namespace ReviewService.API.Controllers
{
    public class ReviewsController : ApiControllerBase
    {
        [HttpGet("product/{productId}")]
        public async Task<ActionResult<PagedList<ReviewDto>>> GetReviewsByProduct(
            string productId,
            [FromQuery] string? status = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string sortBy = "CreatedAt",
            [FromQuery] bool sortDescending = true)
        {
            ReviewStatus? reviewStatus = null;
            if (!string.IsNullOrEmpty(status) && Enum.TryParse<ReviewStatus>(status, out var parsedStatus))
            {
                reviewStatus = parsedStatus;
            }

            var query = new GetReviewsByProductQuery(productId, reviewStatus, page, pageSize, sortBy, sortDescending);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }


        [HttpGet("user/{userId}")]
        public async Task<ActionResult<PagedList<ReviewDto>>> GetReviewsByUser(
            string userId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var query = new GetReviewsByAuthorQuery(userId, page, pageSize);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDto>> GetReview(string id)
        {
            var query = new GetReviewByIdQuery(id);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateReview(CreateReviewCommand command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateReview(string id, UpdateReviewCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReview(string id)
        {
            var command = new DeleteReviewCommand(id);
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPost("{id}/approve")]
        public async Task<ActionResult> ApproveReview(string id)
        {
            var command = new ApproveReviewCommand(id);
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPost("{id}/reject")]
        public async Task<ActionResult> RejectReview(string id, [FromBody] string? reason = null)
        {
            var command = new RejectReviewCommand(id, reason);
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPost("{id}/helpful")]
        public async Task<ActionResult> MarkHelpful(string id)
        {
            var command = new AddReviewReactionCommand(id, true);
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPost("{id}/unhelpful")]
        public async Task<ActionResult> MarkUnhelpful(string id)
        {
            var command = new AddReviewReactionCommand(id, false);
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpGet("product/{productId}/stats")]
        public async Task<ActionResult<ProductRatingStatsDto>> GetProductRatingStats(string productId)
        {
            var query = new GetProductRatingStatsQuery(productId);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }
    }

}
