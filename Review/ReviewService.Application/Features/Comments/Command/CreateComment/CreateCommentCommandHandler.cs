using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReviewService.Application.Common;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Domain.Common;
using ReviewService.Domain.Entities;
using ReviewService.Domain.ValueObjects;

namespace ReviewService.Application.Features.Comments.Command.CreateComment
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Result<string>>
    {
        private readonly IApplicationDbContext _context;
        public CreateCommentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<string>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var discussion = await _context.Discussions.FirstOrDefaultAsync(d => d.Id == request.DiscussionId && !d.IsDeleted, cancellationToken);

                if (discussion == null)
                {
                    return Result.Failure<string>("Discussion not found");
                }

                if (!string.IsNullOrEmpty(request.ParentCommentId))
                { 
                    var parentComment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == request.ParentCommentId && !c.IsDeleted, cancellationToken); ;

                    if (parentComment == null)
                        return Result.Failure<string>("Parent comment not found");
                }

                var content = new Content(request.ContentText);
                var email = new Email(request.AuthorEmail);
                var userInfo = new UserInfo(request.AuthorUserId, request.AuthorUserName, email);

                var comment = new Comment(content, userInfo, request.DiscussionId, request.ParentCommentId);

                _context.Comments.Add(comment);

                if (string.IsNullOrEmpty(request.ParentCommentId))
                {
                    discussion.IncrementCommentCount();
                }
                else
                {
                    var parentComment = await _context.Comments
                        .FirstOrDefaultAsync(c => c.Id == request.ParentCommentId && !c.IsDeleted, cancellationToken);
                    if (parentComment != null)
                    {
                        parentComment.IncrementReplyCount();
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(comment.Id);
            }
            catch (DomainException ex)
            {
                return Result.Failure<string>(ex.Message);
            }
        }
    }
}
