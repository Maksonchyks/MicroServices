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
using ReviewService.Application.Features.Comments.DTOs;

namespace ReviewService.Application.Features.Comments.Queries.GetCommentReplies
{
    public class GetCommentRepliesQueryHandler : IRequestHandler<GetCommentRepliesQuery, Result<PagedList<CommentDto>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCommentRepliesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<CommentDto>>> Handle(GetCommentRepliesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Comments
                .Where(c => c.ParentCommentId == request.ParentCommentId && !c.IsDeleted);

            var totalCount = await query.CountAsync(cancellationToken);

            var replies = await query
                .OrderBy(c => c.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<CommentDto>>(replies);
            var pagedResult = new PagedList<CommentDto>(dtos, totalCount, request.Page, request.PageSize);

            return Result.Success(pagedResult);
        }
    }

}
