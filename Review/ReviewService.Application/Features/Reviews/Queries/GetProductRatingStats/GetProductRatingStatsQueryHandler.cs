using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Common;
using ReviewService.Domain.Entities;
using ReviewService.Application.Features.Reviews.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ReviewService.Application.Features.Reviews.Queries.GetProductRatingStats
{
    public class GetProductRatingStatsQueryHandler : IRequestHandler<GetProductRatingStatsQuery, Result<ProductRatingStatsDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetProductRatingStatsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<ProductRatingStatsDto>> Handle(GetProductRatingStatsQuery request, CancellationToken cancellationToken)
        {
            var approvedReviews = await _context.Reviews
                .Where(r => r.ProductId == request.ProductId &&
                           !r.IsDeleted &&
                           r.Status == ReviewStatus.Approved)
                .ToListAsync(cancellationToken);

            if (!approvedReviews.Any())
                return Result.Success(new ProductRatingStatsDto { ProductId = request.ProductId });

            var averageRating = approvedReviews.Average(r => r.Rating.Score);
            var totalReviews = approvedReviews.Count;
            var ratingDistribution = approvedReviews
                .GroupBy(r => r.Rating.Score)
                .ToDictionary(g => g.Key, g => g.Count());

            var verifiedPurchaseCount = approvedReviews.Count(r => r.IsVerifiedPurchase);
            var totalHelpful = approvedReviews.Sum(r => r.HelpfulCount);
            var totalUnhelpful = approvedReviews.Sum(r => r.UnhelpfulCount);

            return Result.Success(new ProductRatingStatsDto
            {
                ProductId = request.ProductId,
                AverageRating = Math.Round(averageRating, 2),
                TotalReviews = totalReviews,
                RatingDistribution = ratingDistribution,
                VerifiedPurchaseCount = verifiedPurchaseCount,
                TotalHelpfulVotes = totalHelpful,
                TotalUnhelpfulVotes = totalUnhelpful,
                HelpfulnessScore = totalHelpful - totalUnhelpful
            });
        }
    }

}
