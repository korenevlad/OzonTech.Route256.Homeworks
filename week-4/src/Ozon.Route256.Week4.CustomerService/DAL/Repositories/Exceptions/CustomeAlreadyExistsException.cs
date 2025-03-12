namespace Ozon.Route256.Week4.CustomerService.DAL.Repositories.Exceptions;

public sealed class CustomerAlreadyExistsException(string message) : Exception(message);