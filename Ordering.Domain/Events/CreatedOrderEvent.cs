﻿using Ordering.Domain.Abstractions;
using Ordering.Domain.Models;

namespace Ordering.Domain.Events;

public class CreatedOrderEvent(Order order) : IDomainEvent
{
    public Order Order { get; set; } = order;
}