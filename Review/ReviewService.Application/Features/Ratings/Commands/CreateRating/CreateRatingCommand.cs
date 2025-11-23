using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Application.Common.Interfaces;

namespace ReviewService.Application.Features.Ratings.Commands.CreateRating
{
    public record CreateRatingCommand(
        string ProductId,
        string UserId,
        int RatingScore,
        int RatingMaxScore,
        string? ReviewId = null
    ) : ICommand<string>;
}
