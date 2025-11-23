using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Common;
using ReviewService.Application.Features.Ratings.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ReviewService.Application.Features.Ratings.Queries.GetProductRatingSummary
{
    public class GetProductRatingSummaryQueryHandler : IRequestHandler<GetProductRatingSummaryQuery, Result<ProductRatingSummaryDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetProductRatingSummaryQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<ProductRatingSummaryDto>> Handle(GetProductRatingSummaryQuery request, CancellationToken cancellationToken)
        {
            var ratings = await _context.RatingEntities
                .Where(r => r.ProductId == request.ProductId && !r.IsDeleted)
                .ToListAsync(cancellationToken);

            if (!ratings.Any())
                return Result.Success(new ProductRatingSummaryDto { ProductId = request.ProductId });

            var averageRating = ratings.Average(r => r.RatingValue.Score);
            var totalRatings = ratings.Count;

            var ratingDistribution = ratings
                .GroupBy(r => r.RatingValue.Score)
                .ToDictionary(g => g.Key, g => g.Count());

            var ratingPercentages = ratingDistribution
                .ToDictionary(kvp => kvp.Key, kvp => (double)kvp.Value / totalRatings * 100);

            var withReviewsCount = ratings.Count(r => r.IsForReview());

            return Result.Success(new ProductRatingSummaryDto
            {
                ProductId = request.ProductId,
                AverageRating = Math.Round(averageRating, 2),
                TotalRatings = totalRatings,
                RatingDistribution = ratingDistribution,
                RatingPercentages = ratingPercentages,
                WithReviewsCount = withReviewsCount,
                WithoutReviewsCount = totalRatings - withReviewsCount
            });
        }
    }

}
