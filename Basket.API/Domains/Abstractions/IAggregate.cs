using Basket.API.Domains.Events;

namespace Basket.API.Domains.Abstractions;

public interface IAggregate<T> : IAggregate, IEntity<T>
{
    
}

public interface IAggregate
{
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }
    public IDomainEvent[] ClearDomainEvent();
}