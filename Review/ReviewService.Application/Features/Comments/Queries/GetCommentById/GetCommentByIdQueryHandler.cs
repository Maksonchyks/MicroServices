using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Common;
using ReviewService.Application.Features.Comments.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ReviewService.Application.Features.Comments.Queries.GetCommentById
{
    public class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, Result<CommentDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCommentByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<CommentDto>> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
            var comment = await _context.Comments
                .FirstOrDefaultAsync(c => c.Id == request.Id && !c.IsDeleted, cancellationToken);
            if (comment == null)
                return Result.Failure<CommentDto>("Comment not found");

            var dto = _mapper.Map<CommentDto>(comment);
            return Result.Success(dto);
        }
    }
}
