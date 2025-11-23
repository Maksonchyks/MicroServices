using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ReviewService.Application.Common.Interfaces;
using ReviewService.Application.Common;
using ReviewService.Domain.Common;
using ReviewService.Domain.Entities;
using ReviewService.Domain.ValueObjects;

namespace ReviewService.Application.Features.Discussions.Commands.CreateDiscussion
{
    public class CreateDiscussionCommandHandler : IRequestHandler<CreateDiscussionCommand, Result<string>>
    {
        private readonly IApplicationDbContext _context;
        public CreateDiscussionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<string>> Handle(CreateDiscussionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var email = new Email(request.AuthorEmail);
                var userInfo = new UserInfo(request.AuthorUserId, request.AuthorUserName, email);

                var discussion = new Discussion
                (
                    request.Title,
                    request.Description,
                    userInfo,
                    request.Category,
                    request.Tags
                );

                await _context.Discussions.AddAsync(discussion);
                return Result.Success(discussion.Id);
            }
            catch (DomainException ex)
            {
                return Result.Failure<string>(ex.Message);
            }
        }
    }
}
