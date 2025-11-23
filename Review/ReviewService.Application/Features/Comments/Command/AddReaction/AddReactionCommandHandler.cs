using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace ReviewService.Application.Features.Comments.Command.AddReaction
{
    public class AddReactionCommandHandler : IRequestHandler<AddReactionCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public AddReactionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(AddReactionCommand request, CancellationToken cancellationToken)
        {
            var comment = await _context.Comments
                .FirstOrDefaultAsync(c => c.Id == request.CommentId && !c.IsDeleted, cancellationToken);
            if (comment == null)
                return Result.Failure("Comment not found");

            if (request.IsLike)
                comment.AddLike();
            else
                comment.AddDislike();

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
