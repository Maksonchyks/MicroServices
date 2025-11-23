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

namespace ReviewService.Application.Features.Reviews.Queries.GetReviewById
{
    public class GetReviewByIdQueryHandler : IRequestHandler<GetReviewByIdQuery, Result<ReviewDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetReviewByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<ReviewDto>> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.Id == request.Id && !r.IsDeleted, cancellationToken);
            if (review == null)
                return Result.Failure<ReviewDto>("Review not found");

            var dto = _mapper.Map<ReviewDto>(review);
            return Result.Success(dto);
        }
    }

}
