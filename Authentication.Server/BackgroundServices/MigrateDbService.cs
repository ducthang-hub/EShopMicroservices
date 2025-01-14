using Authentication.Server.Persistence.DatabaseContext;
using BuildingBlocks.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Server.BackgroundServices;

public class MigrateDbService
    (
        AuthDbContext dbContext,
        ILogger<MigrateDbService> logger
    )
    : IHostedService, IDisposable
{
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        const string funcName = $"{nameof(MigrateDbService)} - {nameof(StartAsync)}";
        try
        {
            Console.WriteLine($"{funcName} Start Migrate Database");

            await dbContext.Database.MigrateAsync(cancellationToken);

            Console.WriteLine($"{funcName} Finish Migrate Database");
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        logger.LogInformation($"{nameof(MigrateDbService)} has disposed!");
    }
}