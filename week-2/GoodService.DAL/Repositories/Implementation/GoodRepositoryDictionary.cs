using System.Collections.Concurrent;
using GoodService.DAL.Dbos;

namespace GoodService.DAL.Repositories.Implementation;
public class GoodRepositoryDictionary : IGoodRepository
{
    private ConcurrentDictionary<Guid, GoodDbo> _goodStorage;
    public GoodRepositoryDictionary()
    {
        _goodStorage = new ConcurrentDictionary<Guid, GoodDbo>();
    }
    public void AddGood(GoodDbo good) 
        => _goodStorage.TryAdd(good.Id, good);

    public IEnumerable<GoodDbo> GetGoodsWithFilters(DateTime? creationDate, GoodType goodType, int? numberStock, int pageNumber, int pageSize)
    {
        var query = _goodStorage.Values.AsQueryable();
        if (creationDate.HasValue) 
            query = query.Where(g => g.CreationDate >= creationDate);

        if (goodType != 0) 
            query = query.Where(g => g.GoodType == goodType );
        
        if (numberStock.HasValue && numberStock > 0)
            query = query.Where(g =>  g.NumberStock == numberStock);

        var goods = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        
        return goods;
    }

    public GoodDbo? GetGoodById(Guid goodId)
        => _goodStorage.GetValueOrDefault(goodId);
}