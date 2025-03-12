using Moq;
using Ozon.Route256.Week4.CustomerService.DAL.Repositories;
using Ozon.Route256.Week4.CustomerService.DAL.Repositories.Exceptions;

namespace Ozon.Route256.Week4.CustomerService.UnitTests.Extensions;
public static class CustomerRepositoryExtensions
{
    public static Mock<ICustomerRepository> CreateCustomerThrowException(
        this Mock<ICustomerRepository> repository, string fullName, long regionId, CustomerAlreadyExistsException exception)
    {
        repository.Setup(
                repo => repo.CreateCustomer(fullName, regionId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);
        return repository;
    }
    
    public static Mock<ICustomerRepository> CreateCustomerReturnId(
        this Mock<ICustomerRepository> repository, string fullName, long regionId, long id)
    {
        repository.Setup(
                repo => repo.CreateCustomer(fullName, regionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(id);
        return repository;
    }
}