using GoodsService.BLL.Domain.Models;

namespace GoodsService.BLL;
public interface IGoodService
{
    Guid AddGood(string name, double price, double weight, GoodType goodType, int numberStock);
    // TODO: параметры могуть быть не обязательными?
    List<Good> GetGoodsWithFilters(DateTime creationDate, GoodType goodType, int numberStock);
    Good GetGoodById(Guid id);
}