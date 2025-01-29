using BuildingBlocks.CQRS.Extensions;
using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.PipelineBehaviors;
using Carter;
using Catalog.API.Extensions;
using Catalog.API.Persistence.DatabaseContext;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCarter()
    .AddMediatR(typeof(Program).Assembly, [
        typeof(ValidationPipelineBehavior<,>),
        typeof(LoggingBehavior<,>)
    ]);

builder.Services.AddDbContextPool<CatalogDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection")));
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services
    .AddEndpointAuthorization(builder.Configuration)
    .AddCustomHttpClient(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler(_ => { });
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapCarter();
app.Run();
