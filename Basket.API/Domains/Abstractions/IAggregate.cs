using Basket.API.Domains.Events;
using BuildingBlock.Messaging.IntegrationEvents;
using MassTransit;

namespace Basket.API.Domains.Abstractions;

public interface IAggregate<T> : IAggregate, IEntity<T>
{
    
}

public interface IAggregate
{
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }
    public IDomainEvent[] ClearDomainEvent();
}