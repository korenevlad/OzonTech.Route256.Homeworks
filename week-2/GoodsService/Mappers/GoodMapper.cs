using GoodsService.BLL.Domain.Models;
using GoodsService.Grps;
using GoodsService.Presentation.Controllers.v1.Models;
using Google.Protobuf.WellKnownTypes;

namespace GoodsService.Mappers;
public static class GoodMapper
{
    public static GetGoodByIdResponseProto ToGetGoodByIdResponseProto(this Good good)
        => new()
        {
            Name = good.Name,
            Price = good.Price,
            Weight = good.Weight,
            GoodType = good.GoodType.ToProto(),
            CreationDate = Timestamp.FromDateTime(good.CreationDate),
            NumberStock = good.NumberStock
        };
    public static GetGoodByIdResponse ToGetGoodByIdResponse(this Good good)
        => new()
        {
            Name = good.Name,
            Price = good.Price,
            Weight = good.Weight,
            GoodType = good.GoodType.ToHttp(),
            CreationDate = good.CreationDate,
            NumberStock = good.NumberStock
        };
}