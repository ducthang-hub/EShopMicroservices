using Discount.GRPC.MappingConfig;
using Discount.GRPC.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using DiscountService = Discount.GRPC.Services.DiscountService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
builder.Services.AddDbContextPool<DiscountDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection")));
// builder.Services.AddHostedService<CouponRpcServer>();
// builder.Services.AddSingleton<IMessageQueueConnectionProvider, MessageQueueConnectionProvider>();
// builder.Services.AddSingleton<IRpcServer<IEnumerable<Coupon>>, RpcServer<IEnumerable<Coupon>>>();

CouponMapping.RegisterMapping();
var app = builder.Build();

app.MapGrpcService<DiscountService>();
app.Run();