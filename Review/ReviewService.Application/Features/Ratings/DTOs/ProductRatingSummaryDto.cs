using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewService.Application.Features.Ratings.DTOs
{
    public class ProductRatingSummaryDto
    {
        public string ProductId { get; set; } = string.Empty;
        public double AverageRating { get; set; }
        public int TotalRatings { get; set; }
        public Dictionary<int, int> RatingDistribution { get; set; } = new();
        public Dictionary<int, double> RatingPercentages { get; set; } = new();
        public int WithReviewsCount { get; set; }
        public int WithoutReviewsCount { get; set; }
        public int OneStarCount => RatingDistribution.GetValueOrDefault(1, 0);
        public int TwoStarCount => RatingDistribution.GetValueOrDefault(2, 0);
        public int ThreeStarCount => RatingDistribution.GetValueOrDefault(3, 0);
        public int FourStarCount => RatingDistribution.GetValueOrDefault(4, 0);
        public int FiveStarCount => RatingDistribution.GetValueOrDefault(5, 0);
    }
}
