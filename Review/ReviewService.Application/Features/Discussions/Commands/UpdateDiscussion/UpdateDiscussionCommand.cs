using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Application.Common.Interfaces;

namespace ReviewService.Application.Features.Discussions.Commands.UpdateDiscussion
{
    public record UpdateDiscussionCommand
        (
        string Id,
        string Title,
        string Description,
        List<string>? Tags
        ) : ICommand<string>;
}
