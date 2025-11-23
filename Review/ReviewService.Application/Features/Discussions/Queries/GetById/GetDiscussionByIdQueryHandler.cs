using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ReviewService.Application.Common;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Features.Discussions.DTOs;

namespace ReviewService.Application.Features.Discussions.Queries.GetById
{
    public class GetDiscussionByIdQueryHandler : IRequestHandler<GetDiscussionByIdQuery, Result<DiscussionDto>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;
        public GetDiscussionByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<DiscussionDto>> Handle(GetDiscussionByIdQuery request, CancellationToken cancellationToken)
        {
            var discussions = await _context.Discussions.FindAsync(request.Id);
            if (discussions == null || discussions.IsDeleted)
                return Result.Failure<DiscussionDto>("Discussions not find");

            var dto = _mapper.Map<DiscussionDto>(discussions);
            return Result.Success(dto);

        }
    }
}
