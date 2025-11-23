using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Common;

namespace ReviewService.Application.Features.Discussions.Commands.DeleteDiscussion
{
    public class DeleteDiscussionCommandHandler : IRequestHandler<DeleteDiscussionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public DeleteDiscussionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteDiscussionCommand request, CancellationToken cancellationToken)
        {
            var discussion = await _context.Discussions.FindAsync(request.Id);

            if (discussion == null)
            {
                return Result.Failure("Discussion not found");
            }

            discussion.Delete();
            await _context.SaveChangesAsync(cancellationToken);
                return Result.Success();
        }
    }
}
