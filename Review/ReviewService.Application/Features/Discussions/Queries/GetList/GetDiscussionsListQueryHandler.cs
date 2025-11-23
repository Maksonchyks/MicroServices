using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReviewService.Application.Common;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Features.Discussions.DTOs;

namespace ReviewService.Application.Features.Discussions.Queries.GetList
{
    public class GetDiscussionsListQueryHandler : IRequestHandler<GetDiscussionsListQuery, Result<PagedList<DiscussionDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;
        public GetDiscussionsListQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<DiscussionDto>>> Handle(GetDiscussionsListQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Discussions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Category))
                query = query.Where(x => x.Category == request.Category);

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                query = query.Where(x =>
                    x.Title.Contains(request.SearchTerm) ||
                    x.Description.Contains(request.SearchTerm)
                );

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<DiscussionDto>>(items);

            var pagedResult = new PagedList<DiscussionDto>(
                dtos,
                totalCount,
                request.Page,
                request.PageSize
            );

            return Result.Success(pagedResult);
        }
    }
}
