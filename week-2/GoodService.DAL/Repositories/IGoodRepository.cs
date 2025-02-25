using GoodService.DAL.Dbos;

namespace GoodService.DAL.Repositories;
public interface IGoodRepository
{
    void AddGood(GoodDbo good);
    IEnumerable<GoodDbo> GetGoodsWithFilters(DateTime creationDate, GoodType goodType, int numberStock);
    GoodDbo? GetGoodById(Guid goodId);
}