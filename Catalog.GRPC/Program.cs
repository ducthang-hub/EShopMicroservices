using Catalog.API.Persistence.DatabaseContext;
using Catalog.GRPC.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDbContextPool<CatalogDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<ProductCatalogService>();
app.Run();