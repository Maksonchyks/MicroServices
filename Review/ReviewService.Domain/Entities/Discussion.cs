using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Domain.Common;
using ReviewService.Domain.ValueObjects;

namespace ReviewService.Domain.Entities
{
    public class Discussion : BaseEntity
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public UserInfo Author { get; private set; }
        public string Category { get; private set; }
        public List<string> Tags { get; private set; }
        public int ViewCount { get; private set; }
        public int CommentCount { get; private set; }
        public DiscussionStatus Status { get; private set; }
        public DateTime? PinnedAt { get; private set; }

        private Discussion()
        {
            Tags = new List<string>();
        }

        public Discussion(string title, string description, UserInfo author, string category, List<string> tags = null)
        {
            ValidateTitle(title);
            ValidateDescription(description);
            ValidateCategory(category);

            Title = title;
            Description = description;
            Author = author ?? throw new ArgumentNullException(nameof(author));
            Category = category;
            Tags = tags ?? new List<string>();
            ViewCount = 0;
            CommentCount = 0;
            Status = DiscussionStatus.Active;
        }

        private static void ValidateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new InvalidDiscussionException("Title cannot be empty");

            if (title.Length < 5)
                throw new InvalidDiscussionException("Title must be at least 5 characters");

            if (title.Length > 200)
                throw new InvalidDiscussionException("Title cannot exceed 200 characters");
        }

        private static void ValidateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new InvalidDiscussionException("Description cannot be empty");

            if (description.Length < 10)
                throw new InvalidDiscussionException("Description must be at least 10 characters");

            if (description.Length > 5000)
                throw new InvalidDiscussionException("Description cannot exceed 5000 characters");
        }

        public void UpdateTitle(string title)
        {
            ValidateTitle(title);
            Title = title;
            UpdatedAt = DateTime.UtcNow;
            Version++;
        }

        public void UpdateDescription(string description)
        {
            ValidateDescription(description);
            Description = description;
            UpdatedAt = DateTime.UtcNow;
            Version++;
        }

        public void UpdateTags(List<string> tags)
        {
            Tags = tags ?? new List<string>();
            UpdatedAt = DateTime.UtcNow;
            Version++;
        }

        public void IncrementViewCount()
        {
            ViewCount++;
            UpdatedAt = DateTime.UtcNow;
        }

        public void IncrementCommentCount()
        {
            CommentCount++;
            UpdatedAt = DateTime.UtcNow;
        }

        public void DecrementCommentCount()
        {
            if (CommentCount > 0)
            {
                CommentCount--;
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void Pin()
        {
            PinnedAt = DateTime.UtcNow;
            Status = DiscussionStatus.Pinned;
            UpdatedAt = DateTime.UtcNow;
            Version++;
        }

        public void Unpin()
        {
            PinnedAt = null;
            Status = DiscussionStatus.Active;
            UpdatedAt = DateTime.UtcNow;
            Version++;
        }

        public void Close()
        {
            Status = DiscussionStatus.Closed;
            UpdatedAt = DateTime.UtcNow;
            Version++;
        }

        public void Delete()
        {
            IsDeleted = true;
            Status = DiscussionStatus.Deleted;
            UpdatedAt = DateTime.UtcNow;
            Version++;
        }
        private static void ValidateCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                throw new InvalidDiscussionException("Category cannot be empty");

            var validCategories = new[] { "Technology", "Science", "Business", "Lifestyle", "Other" };
            if (!validCategories.Contains(category))
                throw new InvalidDiscussionException($"Invalid category. Valid categories: {string.Join(", ", validCategories)}");
        }
        public enum DiscussionStatus
        {
            Active,
            Closed,
            Pinned,
            Deleted
        }

        public class InvalidDiscussionException : DomainException
        {
            public InvalidDiscussionException(string message) : base(message) { }
        }
    }
}
