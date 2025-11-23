using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Common;
using ReviewService.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace ReviewService.Application.Features.Reviews.Commands.AddReviewAttachment
{
    public class AddReviewAttachmentCommandHandler : IRequestHandler<AddReviewAttachmentCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public AddReviewAttachmentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(AddReviewAttachmentCommand request, CancellationToken cancellationToken)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.Id == request.ReviewId && !r.IsDeleted, cancellationToken);
            if (review == null)
                return Result.Failure("Review not found");

            try
            {
                review.AddAttachment(request.AttachmentUrl);
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
