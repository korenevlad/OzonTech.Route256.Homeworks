using GoodService.DAL.Dbos;
using GoodTypeDAL = GoodService.DAL.Dbos.GoodType;
using GoodTypeBLL = GoodsService.BLL.Domain.Models.GoodType;

namespace GoodsService.Mappers;
public class GoodTypeMapper
{
    public static GoodTypeBLL ToBll(this GoodTypeDAL goodTypeDAL)
    {
        return (GoodTypeBLL)goodTypeDAL;
    }
    public static GoodTypeDAL ToDal(this GoodTypeBLL goodTypeBLL)
    {
        return (GoodTypeDAL)goodTypeBLL;
    }
}