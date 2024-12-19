using Ordering.Domain.Abstractions;

namespace Ordering.Domain.Events;

public class TestEvent(Guid guid) : IDomainEvent
{
    public Guid Guid { get; set; } = guid;
}