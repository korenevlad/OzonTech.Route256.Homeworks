namespace GoodsService.Presentation.Controllers.v1.Models;

public class AddGoodRequest
{
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public double Weight { get; set; }
    public GoodType GoodType { get; set; }
    public int NumberStock { get; set; }
}