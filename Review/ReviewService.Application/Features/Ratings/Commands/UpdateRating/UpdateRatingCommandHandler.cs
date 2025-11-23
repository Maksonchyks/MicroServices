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

namespace ReviewService.Application.Features.Ratings.Commands.UpdateRating
{
    public class UpdateRatingCommandHandler : IRequestHandler<UpdateRatingCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public UpdateRatingCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateRatingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var ratingEntity = await _context.RatingEntities
                    .FirstOrDefaultAsync(r => r.Id == request.Id && !r.IsDeleted, cancellationToken);
                if (ratingEntity == null)
                    return Result.Failure("Rating not found");

                var newRating = new Rating(request.RatingScore, request.RatingMaxScore);
                ratingEntity.UpdateRating(newRating);

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
