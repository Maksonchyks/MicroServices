using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using ReviewService.Application.Features.Ratings.Commands.UpdateRating;

namespace ReviewService.Application.Features.Ratings.Validator
{
    public class UpdateRatingCommandValidator : AbstractValidator<UpdateRatingCommand>
    {
        public UpdateRatingCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Rating ID is required");

            RuleFor(x => x.RatingScore)
                .InclusiveBetween(1, 5).WithMessage("Rating score must be between 1 and 5");

            RuleFor(x => x.RatingMaxScore)
                .Equal(5).WithMessage("Rating max score must be 5");
        }
    }

}
