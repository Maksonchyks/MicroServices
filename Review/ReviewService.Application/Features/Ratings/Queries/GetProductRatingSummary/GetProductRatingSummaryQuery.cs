using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Features.Ratings.DTOs;

namespace ReviewService.Application.Features.Ratings.Queries.GetProductRatingSummary
{
    public record GetProductRatingSummaryQuery(string ProductId) : IQuery<ProductRatingSummaryDto>;

}
