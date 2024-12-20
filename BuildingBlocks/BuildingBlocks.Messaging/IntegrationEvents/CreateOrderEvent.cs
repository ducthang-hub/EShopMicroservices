﻿namespace BuildingBlock.Messaging.IntegrationEvents;

public record CreateOrderEvent : IntegrationEvent
{
    public string UserId { get; set; }
    public double TotalPrice { get; set; }
}