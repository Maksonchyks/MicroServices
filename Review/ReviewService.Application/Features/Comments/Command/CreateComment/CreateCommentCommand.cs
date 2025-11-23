using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ReviewService.Application.Common.Interfaces;

namespace ReviewService.Application.Features.Comments.Command.CreateComment
{
    public record CreateCommentCommand
        (
        string ContentText,
        string AuthorUserId,
        string AuthorUserName,
        string AuthorEmail,
        string DiscussionId,
        string? ParentCommentId = null
        ) : ICommand<string>;
}
