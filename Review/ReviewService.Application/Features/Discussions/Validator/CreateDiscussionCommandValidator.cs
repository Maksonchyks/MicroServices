using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using ReviewService.Application.Features.Discussions.Commands.CreateDiscussion;

namespace ReviewService.Application.Features.Discussions.Validator
{
    public class CreateDiscussionCommandValidator : AbstractValidator<CreateDiscussionCommand>
    {
        public CreateDiscussionCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MinimumLength(5).WithMessage("Title must be at least 5 characters")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MinimumLength(10).WithMessage("Description must be at least 10 characters")
                .MaximumLength(5000).WithMessage("Description cannot exceed 5000 characters");

            RuleFor(x => x.AuthorUserId)
                .NotEmpty().WithMessage("AuthorUserId is required");

            RuleFor(x => x.AuthorUserName)
                .NotEmpty().WithMessage("AuthorUserName is required");

            RuleFor(x => x.AuthorEmail)
                .NotEmpty().WithMessage("AuthorEmail is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Category is required")
                .Must(category => new[] { "Technology", "Science", "Business", "Lifestyle", "Other" }.Contains(category))
                .WithMessage("Invalid category");
        }
    }

}
