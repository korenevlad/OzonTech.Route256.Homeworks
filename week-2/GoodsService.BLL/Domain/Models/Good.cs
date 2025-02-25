﻿namespace GoodsService.Domain.Models;

public class Good
{
    public Guid Id { get; set; }
    public double Price { get; set; }
    public double Weight { get; set; }
    public GoodType GoodType { get; set; }
    public int NumberStock { get; set; }
}