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
using ReviewService.Application.Features.Ratings.DTOs;

namespace ReviewService.Application.Features.Ratings.Queries.GetUserRatings
{
    public class GetUserRatingsQueryHandler : IRequestHandler<GetUserRatingsQuery, Result<PagedList<RatingEntityDto>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetUserRatingsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<RatingEntityDto>>> Handle(GetUserRatingsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.RatingEntities
                .Where(r => r.UserId == request.UserId && !r.IsDeleted)
                .OrderByDescending(r => r.RatedAt);

            var totalCount = await query.CountAsync(cancellationToken);

            var ratings = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<RatingEntityDto>>(ratings);
            var pagedResult = new PagedList<RatingEntityDto>(dtos, totalCount, request.Page, request.PageSize);

            return Result.Success(pagedResult);
        }
    }

}
