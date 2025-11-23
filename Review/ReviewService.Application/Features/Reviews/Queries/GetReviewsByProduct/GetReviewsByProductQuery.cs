using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Common;
using ReviewService.Domain.Entities;
using ReviewService.Application.Features.Reviews.DTOs;

namespace ReviewService.Application.Features.Reviews.Queries.GetReviewsByProduct
{
    public record GetReviewsByProductQuery(
        string ProductId,
        ReviewStatus? Status = null,
        int Page = 1,
        int PageSize = 20,
        string SortBy = "CreatedAt",
        bool SortDescending = true
    ) : IQuery<PagedList<ReviewDto>>;
}
