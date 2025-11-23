using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Application.Common.Interfaces;

namespace ReviewService.Application.Features.Reviews.Commands.UpdateReview
{
    public record UpdateReviewCommand(
        string Id,
        string Title,
        string ContentText,
        int RatingScore,
        int RatingMaxScore
    ) : ICommand;
}
