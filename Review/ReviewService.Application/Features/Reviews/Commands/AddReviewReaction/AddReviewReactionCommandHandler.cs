using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace ReviewService.Application.Features.Reviews.Commands.AddReviewReaction
{
    public class AddReviewReactionCommandHandler : IRequestHandler<AddReviewReactionCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public AddReviewReactionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(AddReviewReactionCommand request, CancellationToken cancellationToken)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.Id == request.ReviewId && !r.IsDeleted, cancellationToken);
            if (review == null)
                return Result.Failure("Review not found");

            if (request.IsHelpful)
                review.MarkAsHelpful();
            else
                review.MarkAsUnhelpful();

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }

}
