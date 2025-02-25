using GoodsService.BLL.Domain.Models;
using GoodsService.Grps;
using Google.Protobuf.WellKnownTypes;

namespace GoodsService.Mappers.Grpc;

public static class GoodMapper
{
    public static GetGoodByIdResponseProto ToGetGoodByIdResponseProto(this Good good)
        => new()
        {
            Id = good.Id.ToString(),
            Name = good.Name,
            Price = good.Price,
            Weight = good.Weight,
            GoodType = good.GoodType.ToProto(),
            CreationDate = Timestamp.FromDateTime(good.CreationDate),
            NumberStock = good.NumberStock
        };
}