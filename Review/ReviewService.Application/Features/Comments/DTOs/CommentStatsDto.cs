using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewService.Application.Features.Comments.DTOs
{
    public class CommentStatsDto
    {
        public string DiscussionId { get; set; } = string.Empty;
        public int TotalComments { get; set; }
        public int TotalReplies { get; set; }
        public int TotalLikes { get; set; }
        public int TotalDislikes { get; set; }
        public DateTime? LastCommentDate { get; set; }
        public string? MostActiveAuthor { get; set; }
    }
}
