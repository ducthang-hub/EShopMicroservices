using BuildingBlocks.MessageQueue.ConnectionProvider;
using BuildingBlocks.Protocols.Rpc.RpcServer;
using Discount.GRPC;
using Discount.GRPC.BackgroundServices;
using Discount.GRPC.Domains;
using Discount.GRPC.MappingConfig;
using Discount.GRPC.Persistence.DatabaseContext;
using Discount.GRPC.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDbContextPool<DiscountDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection")));
builder.Services.AddHostedService<CouponRpcServer>();
builder.Services.AddSingleton<IMessageQueueConnectionProvider, MessageQueueConnectionProvider>();
builder.Services.AddSingleton<IRpcServer<IEnumerable<Coupon>>, RpcServer<IEnumerable<Coupon>>>();

CouponMapping.RegisterMapping();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();