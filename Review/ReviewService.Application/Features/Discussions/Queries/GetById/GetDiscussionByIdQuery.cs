using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Features.Discussions.DTOs;

namespace ReviewService.Application.Features.Discussions.Queries.GetById
{
    public record GetDiscussionByIdQuery
        (
        string Id
        ) : IQuery<DiscussionDto>;
}
