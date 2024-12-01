using Basket.API.Domains.Events;

namespace Basket.API.Domains.Abstractions;

public class Aggregate<T> :  Entity<T>, IAggregate<T>
{
    private List<IDomainEvent> _domainEvents = new();
    
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.ToArray();
    
    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    
    public IDomainEvent[] ClearDomainEvent()
    {
        var events = _domainEvents.ToArray();
        _domainEvents.Clear();

        return events;
    }

}