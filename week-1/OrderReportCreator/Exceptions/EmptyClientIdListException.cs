namespace OrderReportCreator.Exceptions;

public class EmptyClientIdListException: BusinessException
{
    public override string Message => $"Список id клиентов пуст.";
}