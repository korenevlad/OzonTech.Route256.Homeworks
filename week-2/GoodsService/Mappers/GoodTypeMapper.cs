using GoodTypeBLL = GoodsService.BLL.Domain.Models.GoodType;
using GoodTypeProto = GoodsService.Grps.GoodType;
using GoodTypeHttp = GoodsService.Presentation.Controllers.v1.Models.GoodType;

namespace GoodsService.Mappers;
public static class GoodTypeMapper
{
    public static GoodTypeProto ToProto(this GoodTypeBLL goodTypeBLL)
        => (GoodTypeProto)goodTypeBLL;
    
    public static GoodTypeHttp ToHttp(this GoodTypeBLL goodTypeBLL)
        => (GoodTypeHttp)goodTypeBLL;
    
    public static GoodTypeBLL ToBll(this GoodTypeHttp goodTypeHttp)
        => (GoodTypeBLL)goodTypeHttp;
    
    public static GoodTypeBLL ToBLL(this GoodTypeProto goodTypeProto)
        => (GoodTypeBLL)goodTypeProto;
}