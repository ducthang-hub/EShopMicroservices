using System.Net;
using Basket.API.Domains;
using Basket.API.Models.DTOs;
using Basket.API.Models.Responses;
using Basket.API.Persistence.Repositories;
using BuildingBlocks.CQRS;
using BuildingBlocks.Helpers;
using BuildingBlocks.Protocols.Rpc.RpcClient;
using Discount.GRPC;
using Google.Protobuf.WellKnownTypes;

namespace Basket.API.Features.Queries.ShoppingCartQueries.GetShoppingCart;

public class GetShoppingCartHandler : IQueryHandler<GetShoppingCartQuery, GetShoppingCartResponse>
{
    private readonly ILogger<GetShoppingCartHandler> _logger;
    private readonly IBasketRepository _basketRepository;
    private readonly IRpcClient<IEnumerable<Coupon>> _rpcClient;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProto;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    
    public GetShoppingCartHandler
    (
        ILogger<GetShoppingCartHandler> logger,
        IBasketRepository basketRepository,
        IRpcClient<IEnumerable<Coupon>> rpcClient,
        DiscountProtoService.DiscountProtoServiceClient discountProto,
        IConfiguration configuration
    )
    {
        var clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
        _httpClient = new HttpClient(clientHandler);
        
        _logger = logger;
        _basketRepository = basketRepository;
        _rpcClient = rpcClient;
        _discountProto = discountProto;
        _configuration = configuration;
    }
    public async Task<GetShoppingCartResponse> Handle(GetShoppingCartQuery request, CancellationToken cancellationToken)
    {
        const string functionName = $"{nameof(GetShoppingCartHandler)} =>";
        var response = new GetShoppingCartResponse();
        
        try
        {
            var cart = await _basketRepository.GetBasketAsync(request.Id, cancellationToken);
            
            // todo: return the old code some day
            // var coupons = await rpcClient.ProcessUnaryAsync("rpc_coupon", cancellationToken);
            // var coupons = discountProto.GetDiscounts(new Empty(), cancellationToken: cancellationToken);
            
            if (cart is null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = $"Cart with user {request.Id} not found";
            }
            else
            {
                var catalogService = _configuration["Services:Catalog.API"];
                _httpClient.BaseAddress = new Uri(catalogService!);
                var getProductResponse = await _httpClient.GetAsync("products", cancellationToken);
                if (getProductResponse.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogError($"{functionName} can not get product");
                    response.Data = cart;
                }
                else
                {
                    getProductResponse.EnsureSuccessStatusCode();

                    var result = await getProductResponse.Content.ReadAsStringAsync(cancellationToken);
                    var product = JsonHelper.Deserialize<GetProductResponse>(result);
                    response.Data = new
                    {
                        Cart = cart,
                        // Coupons = 
                        Prduct = product.Products
                    };
                }
                
                response.Status = HttpStatusCode.OK;

            }
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, $"{functionName} Error: {ex.Message}");
            response.Message = ex.Message;
        }

        return response;
    }
}