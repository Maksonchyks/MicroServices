using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using ReviewService.Application.Common;

namespace ReviewService.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Any())
            {
                var errorMessage = string.Join("; ", failures.Select(f => f.ErrorMessage));
                return CreateValidationResult<TResponse>(errorMessage);
            }

            return await next();
        }

        private static TResult CreateValidationResult<TResult>(string error)
            where TResult : Result
        {
            if (typeof(TResult) == typeof(Result))
                return (Result.Failure(error) as TResult)!;

            var genericType = typeof(TResult).GetGenericArguments()[0];
            var failureMethod = typeof(Result).GetMethod(nameof(Result.Failure))!
                .MakeGenericMethod(genericType);

            return (TResult)failureMethod.Invoke(null, new object[] { error })!;
        }
    }

}
