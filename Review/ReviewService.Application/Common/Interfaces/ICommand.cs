using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ReviewService.Application.Common.Interfaces
{
    public interface ICommand : IRequest<Result>
    {
    }

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
