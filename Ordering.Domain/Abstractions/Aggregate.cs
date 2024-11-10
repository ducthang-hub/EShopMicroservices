namespace Ordering.Domain.Abstractions;

public class Aggregate<T> : Entity<T>, IAggregate<T>
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public IDomainEvent[] ClearDomainEvents()
    {
        var dequeueDomainEvents = _domainEvents.ToArray();
        _domainEvents.Clear();
        return dequeueDomainEvents;
    }
}