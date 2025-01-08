using Authentication.Server.Extensions;
using BuildingBlocks.CQRS.Extensions;
using Carter;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigDatabase(builder.Configuration)
    .ConfigAuthentication()
    .ConfigIdentityServer()
    .AddMediatR(typeof(Program).Assembly)
    .AddCarter();

var app = builder.Build();

app.UseRouting();
app.UseHttpsRedirection();
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();
app.MapCarter();

app.Run();