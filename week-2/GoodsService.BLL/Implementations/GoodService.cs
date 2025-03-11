using GoodService.DAL.Dbos;
using GoodService.DAL.Repositories;
using GoodsService.BLL.Domain.Models;
using GoodsService.BLL.Exceptions;
using GoodsService.BLL.Mappers;
using GoodType = GoodsService.BLL.Domain.Models.GoodType;

namespace GoodsService.BLL.Implementations;

public class GoodService : IGoodService
{
    private readonly IGoodRepository _goodRepository;
    public GoodService(IGoodRepository goodRepository)
    {
        _goodRepository = goodRepository;
    }
    public async Task<Guid> AddGood(string name, double price, double weight, GoodType goodType, int numberStock)
    {
        var goodId = Guid.NewGuid();
        var good = new GoodDbo()
        {
            Id = goodId,
            Name = name,
            Price = price,
            Weight = weight,
            GoodType = goodType.ToDal(),
            CreationDate = DateTime.UtcNow,
            NumberStock = numberStock
        };
        await _goodRepository.AddGood(good);
        return goodId;
    }

    public List<Good> GetGoodsWithFilters(DateTime? creationDate, GoodType goodType, int? numberStock, int pageNumber, int pageSize)
    {
        var goodDtoList = _goodRepository.GetGoodsWithFilters(creationDate, goodType.ToDal(), numberStock, pageNumber, pageSize);
        var goodList = new List<Good>();
        foreach (var goodDto in goodDtoList)
        {
            goodList.Add(goodDto.ToBll());
        }
        return goodList;
    }

    public Good GetGoodById(Guid id)
    {
        var goodDbo = _goodRepository.GetGoodById(id);
        if (goodDbo is null)
        {
            throw new GoodNotFoundException(id);
        }
        return goodDbo.ToBll();
    }
}