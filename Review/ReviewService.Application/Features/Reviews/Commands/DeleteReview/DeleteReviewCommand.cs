using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Application.Common.Interfaces;

namespace ReviewService.Application.Features.Reviews.Commands.DeleteReview
{
    public record DeleteReviewCommand(string Id) : ICommand;

}
