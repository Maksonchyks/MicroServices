using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Domain.Common;
using ReviewService.Domain.ValueObjects;

namespace ReviewService.Domain.Entities
{
    public class Review : BaseEntity
    {
        public string Title { get; private set; }
        public Content Content { get; private set; }
        public Rating Rating { get; private set; }
        public UserInfo Author { get; private set; }
        public string ProductId { get; private set; }
        public int HelpfulCount { get; private set; }
        public int UnhelpfulCount { get; private set; }
        public ReviewStatus Status { get; private set; }
        public bool IsVerifiedPurchase { get; private set; }
        public bool IsEdited { get; private set; }
        public DateTime? EditedAt { get; private set; }
        public List<string> Attachments { get; private set; }

        private Review() { }

        public Review(string title, Content content, Rating rating, UserInfo author, string productId, bool isVerifiedPurchase = false)
        {
            ValidateTitle(title);

            Title = title;
            Content = content ?? throw new ArgumentNullException(nameof(content));
            Rating = rating ?? throw new ArgumentNullException(nameof(rating));
            Author = author ?? throw new ArgumentNullException(nameof(author));
            ProductId = productId ?? throw new ArgumentNullException(nameof(productId));
            IsVerifiedPurchase = isVerifiedPurchase;
            HelpfulCount = 0;
            UnhelpfulCount = 0;
            Status = ReviewStatus.Pending;
            IsEdited = false;
            Attachments = new List<string>();
        }

        public void UpdateTitle(string title)
        {
            if (Status == ReviewStatus.Deleted)
                throw new InvalidReviewException("Cannot edit a deleted review");

            ValidateTitle(title);
            Title = title;
            IsEdited = true;
            EditedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Version++;
        }

        public void UpdateContent(Content content)
        {
            if (Status == ReviewStatus.Deleted)
                throw new InvalidReviewException("Cannot edit a deleted review");

            Content = content ?? throw new ArgumentNullException(nameof(content));
            IsEdited = true;
            EditedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Version++;
        }

        public void UpdateRating(Rating rating)
        {
            if (Status == ReviewStatus.Deleted)
                throw new InvalidReviewException("Cannot edit a deleted review");

            Rating = rating ?? throw new ArgumentNullException(nameof(rating));
            IsEdited = true;
            EditedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Version++;
        }

        public void MarkAsHelpful()
        {
            HelpfulCount++;
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveHelpful()
        {
            if (HelpfulCount > 0)
            {
                HelpfulCount--;
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void MarkAsUnhelpful()
        {
            UnhelpfulCount++;
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveUnhelpful()
        {
            if (UnhelpfulCount > 0)
            {
                UnhelpfulCount--;
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void Approve()
        {
            Status = ReviewStatus.Approved;
            UpdatedAt = DateTime.UtcNow;
            Version++;
        }

        public void Reject(string reason = null)
        {
            Status = ReviewStatus.Rejected;
            UpdatedAt = DateTime.UtcNow;
            Version++;
        }

        public void Delete()
        {
            Status = ReviewStatus.Deleted;
            IsDeleted = true;
            UpdatedAt = DateTime.UtcNow;
            Version++;
        }

        public void AddAttachment(string attachmentUrl)
        {
            if (string.IsNullOrWhiteSpace(attachmentUrl))
                throw new InvalidReviewException("Attachment URL cannot be empty");

            if (Attachments.Count >= 5)
                throw new InvalidReviewException("Cannot add more than 5 attachments");

            Attachments.Add(attachmentUrl);
            UpdatedAt = DateTime.UtcNow;
        }

        public int GetHelpfulness() => HelpfulCount - UnhelpfulCount;

        private static void ValidateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new InvalidReviewException("Title cannot be empty");

            if (title.Length < 5)
                throw new InvalidReviewException("Title must be at least 5 characters");

            if (title.Length > 200)
                throw new InvalidReviewException("Title cannot exceed 200 characters");
        }
    }

    public enum ReviewStatus
    {
        Pending,
        Approved,
        Rejected,
        Deleted
    }

    public class InvalidReviewException : DomainException
    {
        public InvalidReviewException(string message) : base(message) { }
    }
}
