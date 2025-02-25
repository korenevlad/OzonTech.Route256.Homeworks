namespace OrderReportCreator.Exceptions;

public class ClientIdConversionException: BusinessException
{
    private readonly string InvalidClientId;
    public ClientIdConversionException(string invalidClientId)
    {
        InvalidClientId = invalidClientId;
    }
    public override string Message => $"Некорректный id клиента: {InvalidClientId}.";
}