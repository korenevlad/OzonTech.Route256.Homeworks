namespace OrderReportCreator.Exceptions;

public class IncorrectReportFormatException: BusinessException
{
    private string InvalidString;
    public IncorrectReportFormatException(string invalidString)
    {
        InvalidString = invalidString;
    }
    public override string Message => $"Некорректно введённый формат отчёта: {InvalidString}.";
}