using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace ReviewService.Application.Features.Reviews.Commands.DeleteReview
{
    public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public DeleteReviewCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.Id == request.Id && !r.IsDeleted, cancellationToken);
            if (review == null)
                return Result.Failure("Review not found");

            review.Delete();
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }

}
