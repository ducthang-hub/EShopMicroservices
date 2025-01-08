using Basket.API.Extensions;
using BuildingBlocks.Caching;
using BuildingBlocks.CQRS.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Extensions.Extensions;
using Carter;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddMediatR(typeof(Program).Assembly)
    .AddCustomRedisCache(builder.Configuration)
    .ConfigDatabase(builder.Configuration)
    .ConfigGRPC(builder.Configuration)
    .ConfigMessageBroker(builder.Configuration)
    .ConfigDomainRepository()
    .ConfigNewtonSoft()
    .AddBackgroundService();

builder.Services.AddServicesInvocation();

var app = builder.Build();
app.MapCarter();
app.Run();