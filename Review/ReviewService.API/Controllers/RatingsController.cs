using Microsoft.AspNetCore.Mvc;
using ReviewService.Application.Common;
using ReviewService.Application.Features.Ratings.Commands.CreateOrUpdateRating;
using ReviewService.Application.Features.Ratings.Commands.CreateRating;
using ReviewService.Application.Features.Ratings.Commands.DeleteRating;
using ReviewService.Application.Features.Ratings.Commands.UpdateRating;
using ReviewService.Application.Features.Ratings.DTOs;
using ReviewService.Application.Features.Ratings.Queries.GetProductRatings;
using ReviewService.Application.Features.Ratings.Queries.GetProductRatingSummary;
using ReviewService.Application.Features.Ratings.Queries.GetRatingById;
using ReviewService.Application.Features.Ratings.Queries.GetUserRatingForProduct;
using ReviewService.Application.Features.Ratings.Queries.GetUserRatings;

namespace ReviewService.API.Controllers
{
    public class RatingsController : ApiControllerBase
    {
        [HttpGet("product/{productId}")]
        public async Task<ActionResult<PagedList<RatingEntityDto>>> GetProductRatings(
            string productId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50)
        {
            var query = new GetProductRatingsQuery(productId, page, pageSize);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }


        [HttpGet("user/{userId}")]
        public async Task<ActionResult<PagedList<RatingEntityDto>>> GetUserRatings(
            string userId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var query = new GetUserRatingsQuery(userId, page, pageSize);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RatingEntityDto>> GetRating(string id)
        {
            var query = new GetRatingByIdQuery(id);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("product/{productId}/user/{userId}")]
        public async Task<ActionResult<RatingEntityDto>> GetUserRatingForProduct(string productId, string userId)
        {
            var query = new GetUserRatingForProductQuery(productId, userId);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateRating(CreateRatingCommand command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRating(string id, UpdateRatingCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRating(string id)
        {
            var command = new DeleteRatingCommand(id);
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPost("rate")]
        public async Task<ActionResult<string>> CreateOrUpdateRating(CreateOrUpdateRatingCommand command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpGet("product/{productId}/summary")]
        public async Task<ActionResult<ProductRatingSummaryDto>> GetProductRatingSummary(string productId)
        {
            var query = new GetProductRatingSummaryQuery(productId);
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }
    }

}
