using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using ReviewService.Application.Features.Comments.Command.CreateComment;

namespace ReviewService.Application.Features.Comments.Validator
{
    public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator()
        {
            RuleFor(x => x.ContentText)
                .NotEmpty().WithMessage("Content is required")
                .MinimumLength(3).WithMessage("Content must be at least 3 characters")
                .MaximumLength(5000).WithMessage("Content cannot exceed 5000 characters");

            RuleFor(x => x.AuthorUserId)
                .NotEmpty().WithMessage("AuthorUserId is required");

            RuleFor(x => x.AuthorUserName)
                .NotEmpty().WithMessage("AuthorUserName is required");

            RuleFor(x => x.AuthorEmail)
                .NotEmpty().WithMessage("AuthorEmail is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.DiscussionId)
                .NotEmpty().WithMessage("DiscussionId is required");
        }
    }

}
