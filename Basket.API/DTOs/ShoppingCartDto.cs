﻿namespace Basket.API.DTOs;

public class ShoppingCartDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
}