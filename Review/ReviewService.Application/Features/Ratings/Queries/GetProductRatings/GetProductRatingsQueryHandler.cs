using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Common;
using ReviewService.Application.Features.Ratings.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ReviewService.Application.Features.Ratings.Queries.GetProductRatings
{
    public class GetProductRatingsQueryHandler : IRequestHandler<GetProductRatingsQuery, Result<PagedList<RatingEntityDto>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetProductRatingsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<RatingEntityDto>>> Handle(GetProductRatingsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.RatingEntities
                .Where(r => r.ProductId == request.ProductId && !r.IsDeleted)
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
