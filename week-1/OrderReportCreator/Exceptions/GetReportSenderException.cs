namespace OrderReportCreator.Exceptions;

public class GetReportSenderException : BusinessException
{
    private readonly ResponseFormat _notFoundResponseFormat;
    public GetReportSenderException(ResponseFormat responseFormat)
    {
        _notFoundResponseFormat = responseFormat;
    }
    public override string Message => $"Не найден вариант отправки отчет: {_notFoundResponseFormat}.";
}