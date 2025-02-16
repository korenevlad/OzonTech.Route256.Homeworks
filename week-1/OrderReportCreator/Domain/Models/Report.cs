namespace OrderReportCreator.Domain.Models;

public class Report
{
    public long ClientId { get; init; }
    private float _orderSum;
    public float OrderSum
    {
        get => (float)Math.Round(_orderSum, 2);
        init => _orderSum = value;
    }
    public string FavoriteItemName { get; init; }
}