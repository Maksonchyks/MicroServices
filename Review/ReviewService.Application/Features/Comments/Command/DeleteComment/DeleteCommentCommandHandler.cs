using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReviewService.Application.Common;
using ReviewService.Application.Common.Interfaces;

namespace ReviewService.Application.Features.Comments.Command.DeleteComment
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public DeleteCommentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == request.Id && !c.IsDeleted);
            if (comment == null)
            {
                return Result.Failure("Comment not found");
            }

            var discussion = await _context.Discussions.FirstOrDefaultAsync(d => d.Id == comment.Id && !d.IsDeleted, cancellationToken);

            comment.Delete();

            if (discussion != null && !comment.IsReply())
            {
                discussion.DecrementCommentCount();
            }

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
