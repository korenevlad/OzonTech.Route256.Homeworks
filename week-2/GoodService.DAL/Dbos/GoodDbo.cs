namespace GoodService.DAL.Dbos;
public class GoodDbo
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public double Weight { get; set; }
    public GoodType GoodType { get; set; }
    public DateTime CreationDate { get; set; }
    public int NumberStock { get; set; }
}