using GoodService.DAL.Dbos;
using GoodsService.BLL.Domain.Models;

namespace GoodsService.Mappers;

public static class GoodMapper
{
    public static Good ToBll(this GoodDbo goodDbo)
        => new Good()
        {
            Id = goodDbo.Id,
            Price = goodDbo.Price,
            Weight = goodDbo.Weight,
            GoodType = goodDbo.GoodType,
            NumberStock = goodDbo.NumberStock
        };
}