namespace GoodsService.Presentation.Controllers.v1.Models;

public class GetGoodByIdResponse
{
    public string Name { get; set; }
    public double Price { get; set; }
    public double Weight { get; set; }
    public GoodType GoodType { get; set; }
    public DateTime CreationDate { get; set; }
    public int NumberStock { get; set; }
}