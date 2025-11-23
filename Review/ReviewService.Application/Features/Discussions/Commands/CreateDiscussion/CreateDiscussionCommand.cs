using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Application.Common.Interfaces;

namespace ReviewService.Application.Features.Discussions.Commands.CreateDiscussion
{
    public record CreateDiscussionCommand
        (string Title,
        string Description,
        string AuthorUserId,
        string AuthorUserName,
        string AuthorEmail,
        string Category,
        List<string>? Tags) : ICommand<string>;
}
