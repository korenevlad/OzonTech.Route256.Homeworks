namespace OrderReportCreator.Domain.Models;

public interface IClientRepository
{
    public bool TryFindById(long id);
}