using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ReviewService.Application.Common.Interfaces
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
