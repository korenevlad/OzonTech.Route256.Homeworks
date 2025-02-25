using GoodTypeBLL = GoodsService.BLL.Domain.Models.GoodType;
using GoodTypeProto = GoodsService.Grps.GoodType;

namespace GoodsService.Mappers;
public static class GoodTypeMapper
{
    public static GoodTypeProto ToProto(this GoodTypeBLL goodTypeBLL)
        => (GoodTypeProto)goodTypeBLL;

    public static GoodTypeBLL ToBLL(this GoodTypeProto goodTypePres)
        => (GoodTypeBLL)goodTypePres;
}