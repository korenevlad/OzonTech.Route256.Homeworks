namespace OrderReportCreator.Exceptions;

public class FindClientIdException : BusinessException
{
    private readonly long InvalidClientId;
    public FindClientIdException(long invalidClientId)
    {
        InvalidClientId = invalidClientId;
    }
    public override string Message => $"Клиент с id: {InvalidClientId} не был найден в списке всех заказов.";
}