using System.Net;
using BuildingBlocks.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Exceptions.Handler;

public class CustomExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var handlerResponse = exception switch
        {
            InternalServerException => HandleException(exception, HttpStatusCode.InternalServerError),
            ValidationException => HandleException(exception, HttpStatusCode.BadRequest),
            BadRequestException => HandleException(exception, HttpStatusCode.Accepted),
            NotFoundException => HandleException(exception, HttpStatusCode.NotFound),
            _ => HandleException(exception, HttpStatusCode.InternalServerError)
        };
        
        await httpContext.Response.WriteAsJsonAsync(handlerResponse, cancellationToken);
        return true;
    }

    ErrorResponse HandleException(Exception exception, HttpStatusCode statusCode)
    {
        var handlerResponse = new ErrorResponse
        {
            Status = statusCode,
            Message = exception.Message,
            Data = ""
        };

        if (exception is not ValidationException validationException) return handlerResponse;
        var error = validationException.Errors.FirstOrDefault();
        if (error is null) return handlerResponse;
        
        handlerResponse.Message = error.ErrorMessage;
        handlerResponse.Data = new
        {
            InvalidData = error.PropertyName,
            AttempedValue = error.AttemptedValue,
        };

        return handlerResponse;
    }
}