using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewService.Application.Features.Reviews.DTOs
{
    public class ProductRatingStatsDto
    {
        public string ProductId { get; set; } = string.Empty;
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public Dictionary<int, int> RatingDistribution { get; set; } = new();
        public int VerifiedPurchaseCount { get; set; }
        public int TotalHelpfulVotes { get; set; }
        public int TotalUnhelpfulVotes { get; set; }
        public int HelpfulnessScore { get; set; }
    }
}
