namespace OrderReportCreator.Domain.Models;

public class Item
{
    public string Name { get; init; }
    private float _price;
    public float Price
    {
        get => (float)Math.Round(_price, 2);
        init => _price = value;
    }
}