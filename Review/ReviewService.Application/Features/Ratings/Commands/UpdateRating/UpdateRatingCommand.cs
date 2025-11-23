using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Application.Common.Interfaces;

namespace ReviewService.Application.Features.Ratings.Commands.UpdateRating
{
    public record UpdateRatingCommand(
        string Id,
        int RatingScore,
        int RatingMaxScore
    ) : ICommand;
}
