namespace Basket.API.Domains.Events;

public class CheckoutBasketSuccessfullyEvent : IDomainEvent
{
    public Guid CartId { get; set; }
    public IEnumerable<Guid> CheckoutProductIds { get; set; }
}