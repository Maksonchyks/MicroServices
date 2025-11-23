using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Application.Features.Discussions.DTOs;

namespace ReviewService.Application.Features.Comments.DTOs
{
    public class CommentDto
    {
        public string Id { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int WordCount { get; set; }
        public UserInfoDto Author { get; set; } = null!;
        public string DiscussionId { get; set; } = string.Empty;
        public string? ParentCommentId { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public int ReplyCount { get; set; }
        public int Score => LikeCount - DislikeCount;
        public string Status { get; set; } = string.Empty;
        public bool IsEdited { get; set; }
        public DateTime? EditedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsReply => !string.IsNullOrEmpty(ParentCommentId);
    }
}
