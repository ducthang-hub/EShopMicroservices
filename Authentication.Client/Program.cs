using Authentication.Client.Extensions;
using Carter;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigAuthentication();
builder.Services.AddCarter();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

var app = builder.Build();
app.UseExceptionHandler(_ => { });
app.UseHttpsRedirection();
app.MapCarter();
app.Run();
