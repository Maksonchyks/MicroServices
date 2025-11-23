using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Application.Common.Interfaces;

namespace ReviewService.Application.Features.Reviews.Commands.ApproveReview
{
    public record ApproveReviewCommand(string Id) : ICommand;

}
