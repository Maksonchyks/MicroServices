using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Common;
using Microsoft.EntityFrameworkCore;
using ReviewService.Application.Features.Comments.DTOs;

namespace ReviewService.Application.Features.Comments.Queries.GetCommentStats
{
    public class GetCommentStatsQueryHandler : IRequestHandler<GetCommentStatsQuery, Result<CommentStatsDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetCommentStatsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<CommentStatsDto>> Handle(GetCommentStatsQuery request, CancellationToken cancellationToken)
        {
            var totalComments = await _context.Comments
                .CountAsync(c => c.DiscussionId == request.DiscussionId &&
                               !c.IsDeleted &&
                               string.IsNullOrEmpty(c.ParentCommentId), cancellationToken);

            var totalReplies = await _context.Comments
                .CountAsync(c => c.DiscussionId == request.DiscussionId &&
                               !c.IsDeleted &&
                               !string.IsNullOrEmpty(c.ParentCommentId), cancellationToken);

            var totalLikes = await _context.Comments
                .Where(c => c.DiscussionId == request.DiscussionId && !c.IsDeleted)
                .SumAsync(c => c.LikeCount, cancellationToken);

            var totalDislikes = await _context.Comments
                .Where(c => c.DiscussionId == request.DiscussionId && !c.IsDeleted)
                .SumAsync(c => c.DislikeCount, cancellationToken);

            var lastComment = await _context.Comments
                .Where(c => c.DiscussionId == request.DiscussionId && !c.IsDeleted)
                .OrderByDescending(c => c.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);

            var mostActiveAuthor = await _context.Comments
                .Where(c => c.DiscussionId == request.DiscussionId && !c.IsDeleted)
                .GroupBy(c => c.Author.UserName)
                .Select(g => new { Author = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .FirstOrDefaultAsync(cancellationToken);

            return Result.Success(new CommentStatsDto
            {
                DiscussionId = request.DiscussionId,
                TotalComments = totalComments,
                TotalReplies = totalReplies,
                TotalLikes = totalLikes,
                TotalDislikes = totalDislikes,
                LastCommentDate = lastComment?.CreatedAt,
                MostActiveAuthor = mostActiveAuthor?.Author
            });
        }
    }
}
