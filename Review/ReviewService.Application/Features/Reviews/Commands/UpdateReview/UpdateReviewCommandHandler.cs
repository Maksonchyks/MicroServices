using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Common;
using ReviewService.Domain.Common;
using ReviewService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ReviewService.Application.Features.Reviews.Commands.UpdateReview
{
    public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public UpdateReviewCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var review = await _context.Reviews
                    .FirstOrDefaultAsync(r => r.Id == request.Id && !r.IsDeleted, cancellationToken);
                if (review == null)
                    return Result.Failure("Review not found");

                var newContent = new Content(request.ContentText);
                var newRating = new Rating(request.RatingScore, request.RatingMaxScore);

                review.UpdateTitle(request.Title);
                review.UpdateContent(newContent);
                review.UpdateRating(newRating);

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
