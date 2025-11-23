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
using ReviewService.Domain.ValueObjects;

namespace ReviewService.Application.Features.Comments.Command.UpdateComment
{
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, Result>
    {
        private readonly IApplicationDbContext _context; 
        public UpdateCommentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == request.Id && !c.IsDeleted, cancellationToken);

                if (comment == null)
                {
                    return Result.Failure("Comment not found");
                }

                var newContent = new Content(request.ContentText);
                comment.UpdateContent(newContent);

                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(newContent);
            }
            catch (DomainException ex) 
            {
                return Result.Failure(ex.Message);
            }
        }
    }
}
