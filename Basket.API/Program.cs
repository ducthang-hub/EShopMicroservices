using Basket.API.Persistence.DatabaseContext;
using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.PipelineBehaviors;
using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCarter();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddDbContextPool<BasketDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection")));
builder.Services.AddStackExchangeRedisCache(cfg => cfg.Configuration = builder.Configuration.GetConnectionString("Redis"));
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
// builder.Services.AddScoped<IUnitOfRepository, UnitOfRepository>();
// builder.Services.AddScoped<ICommandHandler<CreateCartCommand, CreateCartResponse>, CreateCartHandler>();
// builder.Services.AddScoped<ITest, Test>();

var app = builder.Build();
app.UseExceptionHandler(_ => { });
app.MapCarter();
app.UseHttpsRedirection();
app.Run();
