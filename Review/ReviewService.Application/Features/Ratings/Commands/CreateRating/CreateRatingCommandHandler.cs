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

namespace ReviewService.Application.Features.Ratings.Commands.CreateRating
{
    public class CreateRatingCommandHandler : IRequestHandler<CreateRatingCommand, Result<string>>
    {
        private readonly IApplicationDbContext _context;

        public CreateRatingCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<string>> Handle(CreateRatingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Перевірка чи користувач вже оцінив цей продукт
                var existingRating = await _context.RatingEntities
                    .FirstOrDefaultAsync(r => r.ProductId == request.ProductId &&
                                            r.UserId == request.UserId &&
                                            !r.IsDeleted, cancellationToken);

                if (existingRating != null)
                    return Result.Failure<string>("User has already rated this product");

                var ratingValue = new Rating(request.RatingScore, request.RatingMaxScore);
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
            catch (DomainException ex)
            {
                return Result.Failure<string>(ex.Message);
            }
        }
    }

}
