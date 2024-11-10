using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Commands.CreateOrder;

namespace Ordering.API.Endpoints.CreateOrder;

public class CreateOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/order/create", async ([FromBody] CreateOrderRequest request, IMediator mediator) =>
        {
            var response = new CreateOrderResponse();
            try
            {
                var createOrderCommand = request.Adapt<CreateOrderCommand>();
                response = await mediator.Send(createOrderCommand);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        });
    }
}