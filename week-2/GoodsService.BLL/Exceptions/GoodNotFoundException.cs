namespace GoodsService.BLL.Exceptions;

public class GoodNotFoundException : BusinessException
{
    private Guid _goodId;
    public GoodNotFoundException(Guid goodId)
    {
        _goodId = goodId;
    }
    public override string Message => $"Не найден товар с id {_goodId}!";
}