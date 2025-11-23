using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Common;
using ReviewService.Application.Features.Comments.DTOs;

namespace ReviewService.Application.Features.Comments.Queries.GetCommentReplies
{
    public record GetCommentRepliesQuery(
        string ParentCommentId,
        int Page = 1,
        int PageSize = 20
    ) : IQuery<PagedList<CommentDto>>;
}
