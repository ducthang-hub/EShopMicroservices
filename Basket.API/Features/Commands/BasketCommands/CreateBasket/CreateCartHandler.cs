using System.Net;
using Basket.API.Domains;
using Basket.API.Persistence.DatabaseContext;
using Basket.API.Persistence.Repositories;
using BuildingBlocks.Contracts;
using BuildingBlocks.CQRS;
using BuildingBlocks.Services.Test;
using Discount.GRPC;
using Google.Protobuf.WellKnownTypes;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Exception = System.Exception;

namespace Basket.API.Features.Commands.BasketCommands.CreateBasket;

public class CreateCartCommand : IRequest<CreateCartResponse>;

public class CreateCartResponse : ErrorResponse
{
    public ShoppingCart Cart { get; set; }
    public List<CouponModel> Coupons { get; set; }
};

public class CreateCartHandler : IRequestHandler<CreateCartCommand, CreateCartResponse>
{
    // private readonly ILogger<CreateBasketHandler> _logger;
    // private readonly IUnitOfRepository _unitOfRepository;
    private readonly IDistributedCache _cache;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoClient;
    public CreateCartHandler
    (
        // ILogger<CreateBasketHandler> logger,
        // IUnitOfRepository unitOfRepository
        IDistributedCache cache,
        DiscountProtoService.DiscountProtoServiceClient discountProtoClient
    )
    {
        _discountProtoClient = discountProtoClient;
        _cache = cache;
        // _logger = logger;
        // _unitOfRepository = unitOfRepository;
    }
    public async Task<CreateCartResponse> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        var response = new CreateCartResponse();
        try
        {
            var coupons = _discountProtoClient.GetDiscounts(new Empty(), cancellationToken: cancellationToken);
            var newCart = new ShoppingCart
            {
                UserId = Guid.NewGuid(),
                ProductQuantity = 1
            };
            await _cache.SetStringAsync("Carts", JsonConvert.SerializeObject(newCart), cancellationToken);
            response.Status = HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            // _logger.LogError(ex, $"Error message: {ex.Message}");    
        }
        return response;
    }
}