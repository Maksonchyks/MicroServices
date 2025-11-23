using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Common;
using ReviewService.Domain.Common;

namespace ReviewService.Application.Features.Discussions.Commands.UpdateDiscussion
{
    public class UpdateDiscussionCommandHandler : IRequestHandler<UpdateDiscussionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public UpdateDiscussionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateDiscussionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var discussion = await _context.Discussions.FindAsync(request.Id);
                if (discussion == null)
                    return Result.Failure("Discussion not found");

                discussion.UpdateTitle(request.Title);
                discussion.UpdateDescription(request.Description);
                discussion.UpdateTags(request.Tags);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException ex)
            {
                return Result.Failure(ex.Message);
            }
        }
    }
}
