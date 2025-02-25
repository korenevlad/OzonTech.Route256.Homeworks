using GoodsService.BLL.Repositories;
using GoodsService.BLL.Repositories.Dbos;
using GoodsService.Domain.Models;

namespace GoodsService.BLL.Implementations;

public class GoodService : IGoodService
{
    private readonly IGoodRepository _goodRepository;
    public GoodService(IGoodRepository goodRepository)
    {
        _goodRepository = goodRepository;
    }
    public Guid AddGood(double price, double weight, GoodType goodType, int numberStock)
    {
        var goodId = Guid.NewGuid();
        var good = new GoodDbo()
        {
            Id = goodId,
            Price = price,
            Weight = weight,
            GoodType = goodType,
            CreationDate = DateTime.UtcNow,
            NumberStock = numberStock
        };
        
        // сохранить
        
        return goodId;
    }

    public IEnumerable<Good> GetGoodsWithFilters(DateTime creationDate, GoodType goodType, int numberStock)
    {
        throw new NotImplementedException();
    }

    public Good GetGoodById(Guid id)
    {
        throw new NotImplementedException();
    }
}