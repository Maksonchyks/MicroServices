using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using ReviewService.Application.Features.Comments.Command.AddReaction;

namespace ReviewService.Application.Features.Comments.Validator
{
    public class AddReactionCommandValidator : AbstractValidator<AddReactionCommand>
    {
        public AddReactionCommandValidator()
        {
            RuleFor(x => x.CommentId)
                .NotEmpty().WithMessage("Comment ID is required");
        }
    }
}
