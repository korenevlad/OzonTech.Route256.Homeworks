namespace GoodService.DAL.Repositories.Dbos;
public record GoodDbo
{
    public Guid Id { get; set; }
    public double Price { get; set; }
    public double Weight { get; set; }
    public GoodType GoodType { get; set; }
    public DateTime CreationDate { get; set; }
    public int NumberStock { get; set; }
}