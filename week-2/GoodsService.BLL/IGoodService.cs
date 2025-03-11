using GoodsService.BLL.Domain.Models;

namespace GoodsService.BLL;
public interface IGoodService
{
    Task<Guid> AddGood(string name, double price, double weight, GoodType goodType, int numberStock);
    List<Good> GetGoodsWithFilters(DateTime? creationDate, GoodType goodType, int? numberStock, int pageNumber, int pageSize);
    Good GetGoodById(Guid id);
}