using Authentication.Server.Extensions;
using Carter;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigDatabase(builder.Configuration)
    .ConfigOpenIddict()
    .AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCarter();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

var app = builder.Build();

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapCarter();

app.Run();