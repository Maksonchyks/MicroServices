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

namespace ReviewService.Application.Features.Ratings.Queries.GetRatingById
{
    public class GetRatingByIdQueryHandler : IRequestHandler<GetRatingByIdQuery, Result<RatingEntityDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRatingByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<RatingEntityDto>> Handle(GetRatingByIdQuery request, CancellationToken cancellationToken)
        {
            var ratingEntity = await _context.RatingEntities
                .FirstOrDefaultAsync(r => r.Id == request.Id && !r.IsDeleted, cancellationToken);
            if (ratingEntity == null)
                return Result.Failure<RatingEntityDto>("Rating not found");

            var dto = _mapper.Map<RatingEntityDto>(ratingEntity);
            return Result.Success(dto);
        }
    }

}
