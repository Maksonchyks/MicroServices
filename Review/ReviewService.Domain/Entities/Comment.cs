using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Domain.Common;
using ReviewService.Domain.ValueObjects;

namespace ReviewService.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public Content Content { get; private set; }
        public UserInfo Author { get; private set; }
        public string DiscussionId { get; private set; }
        public string ParentCommentId { get; private set; }
        public int LikeCount { get; private set; }
        public int DislikeCount { get; private set; }
        public int ReplyCount { get; private set; }
        public CommentStatus Status { get; private set; }
        public bool IsEdited { get; private set; }
        public DateTime? EditedAt { get; private set; }

        private Comment() { }

        public Comment(Content content, UserInfo author, string discussionId, string parentCommentId = null)
        {
            if (string.IsNullOrWhiteSpace(discussionId))
                throw new InvalidCommentException("DiscussionId cannot be empty");

            Content = content ?? throw new ArgumentNullException(nameof(content));
            Author = author ?? throw new ArgumentNullException(nameof(author));
            DiscussionId = discussionId;
            ParentCommentId = parentCommentId;
            LikeCount = 0;
            DislikeCount = 0;
            ReplyCount = 0;
            Status = CommentStatus.Active;
            IsEdited = false;
        }

        public void UpdateContent(Content newContent)
        {
            if (Status == CommentStatus.Deleted)
                throw new InvalidCommentException("Cannot edit a deleted comment");

            Content = newContent ?? throw new ArgumentNullException(nameof(newContent));
            IsEdited = true;
            EditedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Version++;
        }

        public void AddLike()
        {
            LikeCount++;
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveLike()
        {
            if (LikeCount > 0)
            {
                LikeCount--;
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void AddDislike()
        {
            DislikeCount++;
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveDislike()
        {
            if (DislikeCount > 0)
            {
                DislikeCount--;
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void IncrementReplyCount()
        {
            ReplyCount++;
            UpdatedAt = DateTime.UtcNow;
        }

        public void DecrementReplyCount()
        {
            if (ReplyCount > 0)
            {
                ReplyCount--;
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void Delete()
        {
            Status = CommentStatus.Deleted;
            IsDeleted = true;
            UpdatedAt = DateTime.UtcNow;
            Version++;
        }

        public bool IsReply() => !string.IsNullOrEmpty(ParentCommentId);

        public int GetScore() => LikeCount - DislikeCount;
    }

    public enum CommentStatus
    {
        Active,
        Deleted,
        Flagged
    }

    public class InvalidCommentException : DomainException
    {
        public InvalidCommentException(string message) : base(message) { }
    }

}
