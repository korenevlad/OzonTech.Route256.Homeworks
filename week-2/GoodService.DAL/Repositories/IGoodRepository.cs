using GoodService.DAL.Dbos;

namespace GoodService.DAL.Repositories;
public interface IGoodRepository
{
    void AddGood(GoodDbo good);
    IEnumerable<GoodDbo> GetGoodsWithFilters(DateTime? creationDate, GoodType goodType, int? numberStock, int pageNumber, int pageSize);
    GoodDbo? GetGoodById(Guid goodId);
}