using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Helpers;

public static class ErrorHelper
{
    public static void LogError(this Exception ex, ILogger logger)
    {
        logger.LogError(ex, $"Error: {ex.Message}");
    }
}