using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Common;
using ReviewService.Application.Features.Reviews.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ReviewService.Application.Features.Reviews.Queries.GetReviewsByProduct
{
    public class GetReviewsByProductQueryHandler : IRequestHandler<GetReviewsByProductQuery, Result<PagedList<ReviewDto>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetReviewsByProductQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<ReviewDto>>> Handle(GetReviewsByProductQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Reviews
                .Where(r => r.ProductId == request.ProductId && !r.IsDeleted);

            if (request.Status.HasValue)
            {
                query = query.Where(r => r.Status == request.Status.Value);
            }

            query = request.SortBy.ToLower() switch
            {
                "rating" => request.SortDescending
                    ? query.OrderByDescending(r => r.Rating.Score)
                    : query.OrderBy(r => r.Rating.Score),
                "helpfulness" => request.SortDescending
                    ? query.OrderByDescending(r => r.GetHelpfulness())
                    : query.OrderBy(r => r.GetHelpfulness()),
                "createdat" or _ => request.SortDescending
                    ? query.OrderByDescending(r => r.CreatedAt)
                    : query.OrderBy(r => r.CreatedAt)
            };

            var totalCount = await query.CountAsync(cancellationToken);

            var reviews = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<ReviewDto>>(reviews);
            var pagedResult = new PagedList<ReviewDto>(dtos, totalCount, request.Page, request.PageSize);

            return Result.Success(pagedResult);
        }
    }

}
