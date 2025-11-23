using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Application.Common.Interfaces;

namespace ReviewService.Application.Features.Comments.Command.UpdateComment
{
    public record UpdateCommentCommand
        (
        string Id,
        string ContentText
        ) : ICommand;

}
