using GoodTypeDAL = GoodService.DAL.Dbos.GoodType;
using GoodTypeBLL = GoodsService.BLL.Domain.Models.GoodType;

namespace GoodsService.BLL.Mappers;
public static class GoodTypeMapper
{
    public static GoodTypeBLL ToBll(this GoodTypeDAL goodTypeDAL)
        => (GoodTypeBLL)goodTypeDAL;

    public static GoodTypeDAL ToDal(this GoodTypeBLL goodTypeBLL)
        => (GoodTypeDAL)goodTypeBLL;
}