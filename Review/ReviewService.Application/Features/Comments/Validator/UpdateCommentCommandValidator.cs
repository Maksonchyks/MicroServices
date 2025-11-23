using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using ReviewService.Application.Features.Comments.Command.UpdateComment;

namespace ReviewService.Application.Features.Comments.Validator
{
    public class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
    {
        public UpdateCommentCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Comment ID is required");

            RuleFor(x => x.ContentText)
                .NotEmpty().WithMessage("Content is required")
                .MinimumLength(3).WithMessage("Content must be at least 3 characters")
                .MaximumLength(5000).WithMessage("Content cannot exceed 5000 characters");
        }
    }
}
