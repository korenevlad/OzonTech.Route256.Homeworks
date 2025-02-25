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

    public IEnumerable<GoodDbo> GetGoodsWithFilters(DateTime creationDate, GoodType goodType, int numberStock)
    {
        return _goodStorage.Values
            .Where(g => g.CreationDate >= creationDate 
                        && g.GoodType == goodType 
                        && g.NumberStock == numberStock);
    }

    public GoodDbo? GetGoodById(Guid goodId)
        => _goodStorage.GetValueOrDefault(goodId);
}