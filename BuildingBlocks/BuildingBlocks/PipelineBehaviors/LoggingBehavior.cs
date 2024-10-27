using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BuildingBlocks.PipelineBehaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Handle Request = {typeof(TRequest).Name}, Response = {typeof(TResponse).Name} RequestData = {JsonConvert.SerializeObject(request)}");

        var response = await next();
        
        logger.LogInformation($"Handle Response = {typeof(TResponse).Name}, ResponseData = {JsonConvert.SerializeObject(response)}");

        return response;
    }
}