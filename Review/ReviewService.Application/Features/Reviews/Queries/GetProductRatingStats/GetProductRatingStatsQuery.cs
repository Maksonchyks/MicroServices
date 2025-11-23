using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Features.Reviews.DTOs;

namespace ReviewService.Application.Features.Reviews.Queries.GetProductRatingStats
{
    public record GetProductRatingStatsQuery(string ProductId) : IQuery<ProductRatingStatsDto>;

}
