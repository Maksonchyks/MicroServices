using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Domain.Common;
using ReviewService.Domain.ValueObjects;

namespace ReviewService.Domain.Entities
{
    public class RatingEntity : BaseEntity
    {
        public string ProductId { get; private set; }
        public string UserId { get; private set; }
        public Rating RatingValue { get; private set; }
        public string ReviewId { get; private set; }
        public DateTime RatedAt { get; private set; }

        private RatingEntity() { }

        public RatingEntity(string productId, string userId, Rating ratingValue, string reviewId = null)
        {
            if (string.IsNullOrWhiteSpace(productId))
                throw new InvalidRatingEntityException("ProductId cannot be empty");

            if (string.IsNullOrWhiteSpace(userId))
                throw new InvalidRatingEntityException("UserId cannot be empty");

            ProductId = productId;
            UserId = userId;
            RatingValue = ratingValue ?? throw new ArgumentNullException(nameof(ratingValue));
            ReviewId = reviewId;
            RatedAt = DateTime.UtcNow;
        }

        public void UpdateRating(Rating newRating)
        {
            RatingValue = newRating ?? throw new ArgumentNullException(nameof(newRating));
            UpdatedAt = DateTime.UtcNow;
            Version++;
        }

        public bool IsForReview() => !string.IsNullOrEmpty(ReviewId);
    }

    public class InvalidRatingEntityException : DomainException
    {
        public InvalidRatingEntityException(string message) : base(message) { }
    }

}
