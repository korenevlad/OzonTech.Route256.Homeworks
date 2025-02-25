using GoodService.DAL.Dbos;
using GoodsService.BLL.Domain.Models;

namespace GoodsService.BLL.Mappers;
public static class GoodMapper
{
    public static Good ToBll(this GoodDbo goodDbo)
        => new()
        {
            Id = goodDbo.Id,
            Price = goodDbo.Price,
            Weight = goodDbo.Weight,
            GoodType = GoodTypeMapper.ToBll(goodDbo.GoodType),
            CreationDate = goodDbo.CreationDate,
            NumberStock = goodDbo.NumberStock
        };
}