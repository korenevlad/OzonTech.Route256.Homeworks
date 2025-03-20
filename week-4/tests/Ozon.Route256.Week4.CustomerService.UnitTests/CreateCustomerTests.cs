using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Ozon.Route256.Week4.CustomerService.DAL.Repositories.Exceptions;
using Ozon.Route256.Week4.CustomerService.Domain.Services.CreateCustomer;
using Ozon.Route256.Week4.CustomerService.UnitTests.Extensions;
using Ozon.Route256.Week4.CustomerService.UnitTests.Fakers;
using Ozon.Route256.Week4.CustomerService.UnitTests.Stubs;
using Xunit;

namespace Ozon.Route256.Week4.CustomerService.UnitTests;
public class CreateCustomerTests
{
    [Fact]
    public async Task Handle_UniqueId_CustomerCreatedSuccessfully()
    {
        //Arrange
        var customer = CustomerFaker.Generate().WithId(22);
        var createCustomerCommandRequest = new CreateCustomerCommandRequest(customer.FullName, customer.RegionId);
        
        var service = CreateCustomerStub.Create();
        service.CustomerRepositoryMock.CreateCustomerReturnId(createCustomerCommandRequest.FullName,
            createCustomerCommandRequest.RegionId, customer.Id);
        
        //Act
        var response = await service.Handle(createCustomerCommandRequest, CancellationToken.None);
        
        //Assert
        service.VerifyAll();
        service.VerifyNoOtherCalls();
        response.CustomerId.Should().Be(customer.Id);
    }
    
    [Fact]
    public async Task Handle_SimilarCustomers_ThrowsCustomerAlreadyExistsException()
    {
        //Arrange
        var customer = CustomerFaker.Generate();
        var sharedCreateCustomerCommandRequest = new CreateCustomerCommandRequest(customer.FullName, customer.RegionId);
        
        var service = CreateCustomerStub.Create();
        service.CustomerRepositoryMock.CreateCustomerThrowException(
            sharedCreateCustomerCommandRequest.FullName, sharedCreateCustomerCommandRequest.RegionId, new CustomerAlreadyExistsException(""));

        //Act
        var response = await service.Handle(sharedCreateCustomerCommandRequest, CancellationToken.None);
        
        //Assert
        service.VerifyAll();
        service.VerifyNoOtherCalls();
        // BUG: В бизнес-логике логичнее бросать эксепшн, а не заворачивать его в response
        response.Exception.Should().BeOfType<CustomerAlreadyExistsException>();
    }
}