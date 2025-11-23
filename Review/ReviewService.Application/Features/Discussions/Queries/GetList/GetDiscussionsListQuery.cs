using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Application.Common;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Features.Discussions.DTOs;

namespace ReviewService.Application.Features.Discussions.Queries.GetList
{
    public record GetDiscussionsListQuery(
        string? Category = null,
        string? SearchTerm = null,
        int Page = 1,
        int PageSize = 20
    ) : IQuery<PagedList<DiscussionDto>>;
}
