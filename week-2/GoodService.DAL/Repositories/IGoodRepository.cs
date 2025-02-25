using GoodsService.BLL.Repositories.Dbos;

namespace GoodsService.BLL.Repositories;
public interface IGoodRepository
{
    void AddGood(GoodDbo good);
    
}