using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Application.Features.Discussions.DTOs;

namespace ReviewService.Application.Features.Reviews.DTOs
{
    public class ReviewDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int WordCount { get; set; }
        public RatingDto Rating { get; set; } = null!;
        public UserInfoDto Author { get; set; } = null!;
        public string ProductId { get; set; } = string.Empty;
        public int HelpfulCount { get; set; }
        public int UnhelpfulCount { get; set; }
        public int Helpfulness => HelpfulCount - UnhelpfulCount;
        public string Status { get; set; } = string.Empty;
        public bool IsVerifiedPurchase { get; set; }
        public bool IsEdited { get; set; }
        public DateTime? EditedAt { get; set; }
        public List<string> Attachments { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
