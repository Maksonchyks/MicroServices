using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Application.Common.Interfaces;

namespace ReviewService.Application.Features.Comments.Command.AddReaction
{
    public record AddReactionCommand(
        string CommentId,
        bool IsLike 
    ) : ICommand;
}
