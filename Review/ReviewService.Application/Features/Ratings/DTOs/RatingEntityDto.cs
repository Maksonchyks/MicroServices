using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Application.Features.Reviews.DTOs;

namespace ReviewService.Application.Features.Ratings.DTOs
{
    public class RatingEntityDto
    {
        public string Id { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public RatingDto RatingValue { get; set; } = null!;
        public string? ReviewId { get; set; }
        public DateTime RatedAt { get; set; }
        public bool IsForReview => !string.IsNullOrEmpty(ReviewId);
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
