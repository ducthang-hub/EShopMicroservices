using Authentication.Server.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigDatabase(builder.Configuration)
    .ConfigOpenIddict();

var app = builder.Build();

app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();
app.Run();