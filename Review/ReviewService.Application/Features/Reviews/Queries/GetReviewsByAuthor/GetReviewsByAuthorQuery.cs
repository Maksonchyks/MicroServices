using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Common;
using ReviewService.Application.Features.Reviews.DTOs;

namespace ReviewService.Application.Features.Reviews.Queries.GetReviewsByAuthor
{
    public record GetReviewsByAuthorQuery(
        string AuthorUserId,
        int Page = 1,
        int PageSize = 20
    ) : IQuery<PagedList<ReviewDto>>;
}
