namespace OrderReportCreator.Exceptions;

public class GetClientIdException : BusinessException
{
    private long InvalidClientId;
    public GetClientIdException(long invalidClientId)
    {
        InvalidClientId = invalidClientId;
    }
    public override string Message => $"Клиент с id: {InvalidClientId} не был найден в списке всех заказов.";
}