using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using ReviewService.Application.Features.Reviews.Commands.CreateReview;

namespace ReviewService.Application.Features.Reviews.Validator
{
    public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MinimumLength(5).WithMessage("Title must be at least 5 characters")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

            RuleFor(x => x.ContentText)
                .NotEmpty().WithMessage("Content is required")
                .MinimumLength(10).WithMessage("Content must be at least 10 characters")
                .MaximumLength(5000).WithMessage("Content cannot exceed 5000 characters");

            RuleFor(x => x.RatingScore)
                .InclusiveBetween(1, 5).WithMessage("Rating score must be between 1 and 5");

            RuleFor(x => x.RatingMaxScore)
                .Equal(5).WithMessage("Rating max score must be 5");

            RuleFor(x => x.AuthorUserId)
                .NotEmpty().WithMessage("AuthorUserId is required");

            RuleFor(x => x.AuthorUserName)
                .NotEmpty().WithMessage("AuthorUserName is required");

            RuleFor(x => x.AuthorEmail)
                .NotEmpty().WithMessage("AuthorEmail is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ProductId is required");

            RuleFor(x => x.Attachments)
                .Must(attachments => attachments == null || attachments.Count <= 5)
                .WithMessage("Cannot have more than 5 attachments");
        }
    }

}
