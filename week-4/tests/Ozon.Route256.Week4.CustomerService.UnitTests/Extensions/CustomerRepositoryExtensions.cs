using Moq;
using Ozon.Route256.Week4.CustomerService.DAL.Contracts;
using Ozon.Route256.Week4.CustomerService.DAL.Repositories;
using Ozon.Route256.Week4.CustomerService.DAL.Repositories.Exceptions;
using Ozon.Route256.Week4.CustomerService.Domain.Models;
using Ozon.Route256.Week4.CustomerService.Domain.Services.GetCustomers;

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

    public static Mock<ICustomerRepository> GetCustomersQueryReturnsCustomers(
        this Mock<ICustomerRepository> repository, GetCustomersQueryRequest request, CustomerDbRecord[] customers)
    {
        repository.Setup(
                repo => repo.GetCustomers(request.CustomerIds, request.RegionIds, request.FullNames, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customers);
        return repository;
    }
}