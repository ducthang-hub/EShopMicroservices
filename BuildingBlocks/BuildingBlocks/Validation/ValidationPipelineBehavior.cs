using FluentValidation;
using MediatR;

namespace BuildingBlocks.Validation;

public class ValidationPipelineBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationResult = await Task.WhenAll(validators.Select(i => i.ValidateAsync(context, cancellationToken)));

        var failures = validationResult
            .Where(i => i.Errors.Count != 0)
            .SelectMany(i => i.Errors)
            .ToList();

        if (failures.Count != 0)
        {
            throw new ValidationException(failures);
        }
        
        return await next();
    }
}