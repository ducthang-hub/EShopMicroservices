using System.Reflection;
using BuildingBlocks.CQRS.Extensions;
using BuildingBlocks.MassTransit.Extensions;
using Carter;
using Ordering.Application;
using Ordering.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddInfrastructureServices(builder.Configuration)
    .AddMediatR(Assembly.GetExecutingAssembly())
    .AddMessageBroker(builder.Configuration, Assembly.GetExecutingAssembly())
    .AddCarter();

var app = builder.Build();
app.UseHttpsRedirection();
app.MapCarter();
app.Run();