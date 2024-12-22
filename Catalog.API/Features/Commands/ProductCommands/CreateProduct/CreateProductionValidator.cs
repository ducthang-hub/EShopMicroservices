using System.Net;
using FluentValidation;

namespace Catalog.API.Features.Commands.ProductCommands.CreateProduct;

public class CreateProductionValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductionValidator()
    {
        RuleFor(i => i.Name)
            // .Cascade(CascadeMode.Stop)
            .MinimumLength(5)
            .WithMessage("Maximum length for Product name is 5")
            .WithErrorCode(HttpStatusCode.BadRequest.ToString())
            .MaximumLength(50)
            .WithMessage("Maximum length for Product name is 10")
            .WithErrorCode(HttpStatusCode.BadRequest.ToString());
    }
}