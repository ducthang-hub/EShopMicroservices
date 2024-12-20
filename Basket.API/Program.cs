using Basket.API.Extensions;
using BuildingBlocks.Caching;
using BuildingBlocks.Extensions;
using BuildingBlocks.Extensions.Extensions;
using Carter;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .ConfigMediatR()
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