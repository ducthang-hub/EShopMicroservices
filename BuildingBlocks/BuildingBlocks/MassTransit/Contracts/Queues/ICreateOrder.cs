﻿using BuildingBlock.Messaging.IntegrationEvents;

namespace BuildingBlocks.MassTransit.Contracts.Queues;

public interface ICreateOrder
{
    public CreateOrderEvent Content { get; set; }
}