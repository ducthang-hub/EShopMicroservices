using FluentValidation;
using MediatR;
using ValidationException = FluentValidation.ValidationException;

namespace BuildingBlocks.Validation;

public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationResult = await Task.WhenAll(_validators.Select(i => i.ValidateAsync(context, cancellationToken)));

        var failures = validationResult.Where(i => i.Errors.Any())
            .SelectMany(i => i.Errors)
            .ToList();

        if (failures.Any())
        {
            throw new ValidationException(failures);
        }
        
        return await next();
    }
}