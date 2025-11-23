using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewService.Application.Features.Discussions.DTOs
{
    public class DiscussionDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public UserInfoDto Author { get; set; } = null!;
        public string Category { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new();
        public int ViewCount { get; set; }
        public int CommentCount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
