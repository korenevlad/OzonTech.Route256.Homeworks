namespace GoodsService.BLL.Domain.Models;

public class Good
{
    public string? Name { get; set; }
    public double Price { get; set; }
    public double Weight { get; set; }
    public GoodType GoodType { get; set; }
    public DateTime CreationDate { get; set; }
    public int NumberStock { get; set; }
}