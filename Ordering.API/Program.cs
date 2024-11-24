using System.Reflection;
using Carter;
using Ordering.Application;
using Ordering.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices(builder.Configuration, Assembly.GetExecutingAssembly());
builder.Services.AddCarter();

var app = builder.Build();
app.UseHttpsRedirection();
app.MapCarter();
app.Run();