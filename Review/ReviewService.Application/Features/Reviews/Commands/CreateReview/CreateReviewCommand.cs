using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Application.Common.Interfaces;

namespace ReviewService.Application.Features.Reviews.Commands.CreateReview
{
    public record CreateReviewCommand(
        string Title,
        string ContentText,
        int RatingScore,
        int RatingMaxScore,
        string AuthorUserId,
        string AuthorUserName,
        string AuthorEmail,
        string ProductId,
        bool IsVerifiedPurchase = false,
        List<string>? Attachments = null
    ) : ICommand<string>;
}
