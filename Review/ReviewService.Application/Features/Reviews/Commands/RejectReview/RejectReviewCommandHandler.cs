using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace ReviewService.Application.Features.Reviews.Commands.RejectReview
{
    public class RejectReviewCommandHandler : IRequestHandler<RejectReviewCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public RejectReviewCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(RejectReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.Id == request.Id && !r.IsDeleted, cancellationToken);
            if (review == null)
                return Result.Failure("Review not found");

            review.Reject(request.Reason);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }

}
