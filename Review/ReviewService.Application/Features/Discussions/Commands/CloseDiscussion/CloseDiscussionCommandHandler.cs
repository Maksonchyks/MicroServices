using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace ReviewService.Application.Features.Discussions.Commands.CloseDiscussion
{
    public class CloseDiscussionCommandHandler : IRequestHandler<CloseDiscussionCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public CloseDiscussionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(CloseDiscussionCommand request, CancellationToken cancellationToken)
        {
            var discussion = await _context.Discussions
                .FirstOrDefaultAsync(d => d.Id == request.Id && !d.IsDeleted, cancellationToken);
            if (discussion == null)
                return Result.Failure("Discussion not found");

            discussion.Close();
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }

}
