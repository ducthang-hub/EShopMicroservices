using BuildingBlocks.Contracts;
using BuildingBlocks.CQRS;

namespace Basket.API.Features.Commands.MessageCommands.Logging;

public class LoggingResponse : ErrorResponse
{
}

public class LoggingCommand : ICommand<LoggingResponse>
{
    public LoggingRequest LoggingRequest { get; set; }

    public LoggingCommand(LoggingRequest request)
    {
        LoggingRequest = request;
    }
}

public class LoggingRequest
{
    public string Message { get; set; }
    public IEnumerable<string> Severities { get; set; }
}