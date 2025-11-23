using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Common;
using Microsoft.EntityFrameworkCore;
using ReviewService.Application.Features.Reviews.DTOs;

namespace ReviewService.Application.Features.Reviews.Queries.GetReviewsByAuthor
{
    public class GetReviewsByAuthorQueryHandler : IRequestHandler<GetReviewsByAuthorQuery, Result<PagedList<ReviewDto>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetReviewsByAuthorQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<ReviewDto>>> Handle(GetReviewsByAuthorQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Reviews
                .Where(r => r.Author.UserId == request.AuthorUserId && !r.IsDeleted)
                .OrderByDescending(r => r.CreatedAt);

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
