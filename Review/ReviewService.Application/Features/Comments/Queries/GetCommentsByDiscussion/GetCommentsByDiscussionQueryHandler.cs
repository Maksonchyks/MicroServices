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

namespace ReviewService.Application.Features.Comments.Queries.GetCommentsByDiscussion
{
    public class GetCommentsByDiscussionQueryHandler : IRequestHandler<GetCommentsByDiscussionQuery, Result<PagedList<CommentDto>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCommentsByDiscussionQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<CommentDto>>> Handle(GetCommentsByDiscussionQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Comments
                .Where(c => c.DiscussionId == request.DiscussionId && !c.IsDeleted);

            if (!request.IncludeReplies)
            {
                query = query.Where(c => string.IsNullOrEmpty(c.ParentCommentId));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var comments = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<CommentDto>>(comments);
            var pagedResult = new PagedList<CommentDto>(dtos, totalCount, request.Page, request.PageSize);

            return Result.Success(pagedResult);
        }
    }
}
