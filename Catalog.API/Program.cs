using BuildingBlocks.CQRS.Extensions;
using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.PipelineBehaviors;
using Carter;
using Catalog.API.Extensions;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCarter()
    .AddMediatR(typeof(Program).Assembly, [
        typeof(ValidationPipelineBehavior<,>),
        typeof(LoggingBehavior<,>)
    ]);
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services
    .AddDatabaseConnection(builder.Configuration)
    .AddEndpointAuthorization(builder.Configuration)
    .AddCustomHttpClient(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler(_ => { });
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapCarter();
app.Run();
