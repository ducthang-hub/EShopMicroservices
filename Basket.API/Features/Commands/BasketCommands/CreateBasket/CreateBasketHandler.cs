using System.Net;
using Basket.API.Domains;
using Basket.API.Persistence.Repositories;
using Basket.API.Services;
using BuildingBlocks.Contracts;
using MediatR;
using Exception = System.Exception;

namespace Basket.API.Features.Commands.BasketCommands.CreateBasket;

public class CreateBasketResponse : ErrorResponse;

public class CreateBasketCommand : IRequest<CreateBasketResponse>;

public class CreateBasketHandler : IRequestHandler<CreateBasketCommand, CreateBasketResponse>
{
    private readonly ILogger<CreateBasketHandler> _logger;
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly IServiceProvider _serviceProvider;

    public CreateBasketHandler
    (
        ILogger<CreateBasketHandler> logger,
        // IUnitOfRepository unitOfRepository,
        ITest test,
        // BasketDbContext dbContext,
        IServiceProvider serviceProvider
    )
    {
        _logger = logger;
        // _unitOfRepository = unitOfRepository;
        _serviceProvider = serviceProvider;
    }
    public async Task<CreateBasketResponse> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
    {
        var response = new CreateBasketResponse();
        try
        {
            var newCart = new ShoppingCart
            {
                UserId = Guid.NewGuid()
            };
            using var scope = _serviceProvider.CreateScope();
            var unitOfRepository = scope.ServiceProvider.GetService<IUnitOfRepository>();
            await unitOfRepository.ShoppingCartRepository.Add(newCart);
            // dbContext.UserBasket.Add(newBasket);
            // await dbContext.SaveChangesAsync(cancellationToken);
            // response.Data = newBasket;
            response.Status = HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error message: {ex.Message}");    
        }
        return response;
    }
}