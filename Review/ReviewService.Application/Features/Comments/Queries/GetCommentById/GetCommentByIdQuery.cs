using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Features.Comments.DTOs;

namespace ReviewService.Application.Features.Comments.Queries.GetCommentById
{
    public record GetCommentByIdQuery(string Id) : IQuery<CommentDto>;

}
