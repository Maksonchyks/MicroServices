using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace ReviewService.Application.Features.Ratings.Commands.DeleteRating
{
    public class DeleteRatingCommandHandler : IRequestHandler<DeleteRatingCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public DeleteRatingCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteRatingCommand request, CancellationToken cancellationToken)
        {
            var ratingEntity = await _context.RatingEntities
                .FirstOrDefaultAsync(r => r.Id == request.Id && !r.IsDeleted, cancellationToken);
            if (ratingEntity == null)
                return Result.Failure("Rating not found");

            ratingEntity.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }

}
