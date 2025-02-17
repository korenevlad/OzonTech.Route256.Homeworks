namespace OrderReportCreator.Domain.Models;

public class ReportRow
{
    public long ClientId { get; set; }
    private float _orderSum;
    public float OrderSum
    {
        get => (float)Math.Round(_orderSum, 2);
        set => _orderSum = value;
    }
    public string FavoriteItemName { get; set; }
}