namespace OrderReportCreator.Exceptions;

public class EmptyClientIdListException: BusinessException
{
    public EmptyClientIdListException() { }
    public override string Message => $"Список id клиентов пуст.";
}