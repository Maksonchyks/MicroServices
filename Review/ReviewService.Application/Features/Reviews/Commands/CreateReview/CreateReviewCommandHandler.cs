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

namespace ReviewService.Application.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Result<string>>
    {
        private readonly IApplicationDbContext _context;

        public CreateReviewCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<string>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var content = new Content(request.ContentText);
                var rating = new Rating(request.RatingScore, request.RatingMaxScore);
                var email = new Email(request.AuthorEmail);
                var userInfo = new UserInfo(request.AuthorUserId, request.AuthorUserName, email);

                var review = new Review(
                    request.Title,
                    content,
                    rating,
                    userInfo,
                    request.ProductId,
                    request.IsVerifiedPurchase
                );

                if (request.Attachments != null)
                {
                    foreach (var attachment in request.Attachments)
                    {
                        review.AddAttachment(attachment);
                    }
                }

                _context.Reviews.Add(review);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success(review.Id);
            }
            catch (DomainException ex)
            {
                return Result.Failure<string>(ex.Message);
            }
        }
    }
}
