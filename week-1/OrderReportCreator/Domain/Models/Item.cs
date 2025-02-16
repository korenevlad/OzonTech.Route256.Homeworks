namespace OrderReportCreator.Domain.Models;

public class Item
{
    public string Name { get; set; }
    private float _price;
    public float Price
    {
        get => (float)Math.Round(_price, 2);
        set => _price = value;
    }
}