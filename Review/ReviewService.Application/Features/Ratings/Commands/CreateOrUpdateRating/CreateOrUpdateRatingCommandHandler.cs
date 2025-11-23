using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Common;
using ReviewService.Domain.Common;
using ReviewService.Domain.Entities;
using ReviewService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ReviewService.Application.Features.Ratings.Commands.CreateOrUpdateRating
{
    public class CreateOrUpdateRatingCommandHandler : IRequestHandler<CreateOrUpdateRatingCommand, Result<string>>
    {
        private readonly IApplicationDbContext _context;

        public CreateOrUpdateRatingCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<string>> Handle(CreateOrUpdateRatingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Пошук існуючого рейтингу
                var existingRating = await _context.RatingEntities
                    .FirstOrDefaultAsync(r => r.ProductId == request.ProductId &&
                                            r.UserId == request.UserId &&
                                            !r.IsDeleted, cancellationToken);

                var ratingValue = new Rating(request.RatingScore, request.RatingMaxScore);

                if (existingRating != null)
                {
                    // Оновлення існуючого рейтингу
                    existingRating.UpdateRating(ratingValue);
                    await _context.SaveChangesAsync(cancellationToken);
                    return Result.Success(existingRating.Id);
                }
                else
                {
                    // Створення нового рейтингу
                    var ratingEntity = new RatingEntity(
                        request.ProductId,
                        request.UserId,
                        ratingValue,
                        request.ReviewId
                    );

                    _context.RatingEntities.Add(ratingEntity);
                    await _context.SaveChangesAsync(cancellationToken);
                    return Result.Success(ratingEntity.Id);
                }
            }
            catch (DomainException ex)
            {
                return Result.Failure<string>(ex.Message);
            }
        }
    }

}
