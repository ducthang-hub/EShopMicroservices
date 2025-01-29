using BuildingBlocks.HttpClient.Extension;
using BuildingBlocks.MessageQueue.ConnectionProvider;
using BuildingBlocks.Protocols.Rpc.RpcServer;
using Discount.GRPC;
using Discount.GRPC.BackgroundServices;
using Discount.GRPC.Domains;
using Discount.GRPC.MappingConfig;
using Discount.GRPC.Persistence.DatabaseContext;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using DiscountService = Discount.GRPC.Services.DiscountService;

var builder = WebApplication.CreateBuilder(args);
// builder.WebHost.ConfigureKestrel(options =>
// {
//     // Setup a HTTP/2 endpoint without TLS.
//     options.ListenAnyIP(6056, o => o.Protocols = HttpProtocols.Http2);
// });
// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDbContextPool<DiscountDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection")));
// builder.Services.AddHostedService<CouponRpcServer>();
// builder.Services.AddSingleton<IMessageQueueConnectionProvider, MessageQueueConnectionProvider>();
// builder.Services.AddSingleton<IRpcServer<IEnumerable<Coupon>>, RpcServer<IEnumerable<Coupon>>>();

CouponMapping.RegisterMapping();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountService>();
app.Run();