using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Common;
using ReviewService.Application.Features.Comments.DTOs;

namespace ReviewService.Application.Features.Comments.Queries.GetCommentsByDiscussion
{
    public record GetCommentsByDiscussionQuery(
        string DiscussionId,
        int Page = 1,
        int PageSize = 50,
        bool IncludeReplies = false
    ) : IQuery<PagedList<CommentDto>>;
}
