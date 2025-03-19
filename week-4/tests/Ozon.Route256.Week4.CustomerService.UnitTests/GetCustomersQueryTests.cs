using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Ozon.Route256.Week4.CustomerService.Domain.Services.GetCustomers;
using Ozon.Route256.Week4.CustomerService.Extensions.Mapping;
using Ozon.Route256.Week4.CustomerService.UnitTests.Extensions;
using Ozon.Route256.Week4.CustomerService.UnitTests.Fakers;
using Ozon.Route256.Week4.CustomerService.UnitTests.Stubs;
using Xunit;

namespace Ozon.Route256.Week4.CustomerService.UnitTests;
public class GetCustomersQueryTests
{
    [Fact]
    public async Task Handle_SendRequestWithAllFilters_GetAllCustomersWithAllFilters()
    {
        //Arrange
        var customersFaker = CustomerDbRecordFaker.GenerateListForGetCustomersQuery();
        var getCustomersQueryRequestFaker = GetCustomersQueryRequestFaker.GenerateFromCustomers(customersFaker);
        
        var service = GetCustomersQueryStub.Create();
        service.CustomerRepositoryMock.GetCustomersQueryReturnCustomers(getCustomersQueryRequestFaker, customersFaker.ToArray());
        
        //Act
        var response = await service.Handle(getCustomersQueryRequestFaker, CancellationToken.None);

        //Assert
        service.VerifyAll();
        service.VerifyNoOtherCalls();
        response.Customers.Should().BeEquivalentTo(customersFaker.ToArray().ToBll());
    }
    
    [Fact]
    public async Task Handle_SendRequestWithFilterCustomerIds_GetAllCustomersById()
    {
        //Arrange
        var customersFaker = CustomerDbRecordFaker.GenerateListForGetCustomersQuery(includeCustomerIds: false);
        var getCustomersQueryRequestFaker = GetCustomersQueryRequestFaker.GenerateFromCustomers(customersFaker, includeCustomerIds: false);
        
        var service = GetCustomersQueryStub.Create();
        service.CustomerRepositoryMock.GetCustomersQueryReturnCustomers(getCustomersQueryRequestFaker, customersFaker.ToArray());
        
        //Act
        var response = await service.Handle(getCustomersQueryRequestFaker, CancellationToken.None);
    
        //Assert
        service.VerifyAll();
        service.VerifyNoOtherCalls();
        response.Customers.Should().BeEquivalentTo(customersFaker.ToArray().ToBll());
    } 
    
    [Fact]
    public async Task Handle_SendRequestWithFilterRegionIds_GetAllCustomersByRegionId()
    {
        //Arrange
        var customersFaker = CustomerDbRecordFaker.GenerateListForGetCustomersQuery(includeRegionIds: false);
        var getCustomersQueryRequestFaker = GetCustomersQueryRequestFaker.GenerateFromCustomers(customersFaker, includeRegionIds: false);
        
        var service = GetCustomersQueryStub.Create();
        service.CustomerRepositoryMock.GetCustomersQueryReturnCustomers(getCustomersQueryRequestFaker, customersFaker.ToArray());
        
        //Act
        var response = await service.Handle(getCustomersQueryRequestFaker, CancellationToken.None);
    
        //Assert
        service.VerifyAll();
        service.VerifyNoOtherCalls();
        response.Customers.Should().BeEquivalentTo(customersFaker.ToArray().ToBll());
    } 
    
    [Fact]
    public async Task Handle_SendRequestWithFilterFullNames_GetAllCustomersByFullName()
    {
        //Arrange
        var customersFaker = CustomerDbRecordFaker.GenerateListForGetCustomersQuery(includeFullNames: false);
        var getCustomersQueryRequestFaker = GetCustomersQueryRequestFaker.GenerateFromCustomers(customersFaker, includeFullNames: false);
        
        var service = GetCustomersQueryStub.Create();
        service.CustomerRepositoryMock.GetCustomersQueryReturnCustomers(getCustomersQueryRequestFaker, customersFaker.ToArray());
        
        //Act
        var response = await service.Handle(getCustomersQueryRequestFaker, CancellationToken.None);
    
        //Assert
        service.VerifyAll();
        service.VerifyNoOtherCalls();
        response.Customers.Should().BeEquivalentTo(customersFaker.ToArray().ToBll());
    }
    
    [Fact]
    public async Task Handle_SendRequestWithoutFilters_GetAllCustomersWithoutFilters()
    {
        //Arrange
        var customersFaker = CustomerDbRecordFaker.GenerateListForGetCustomersQuery(includeCustomerIds: false, includeRegionIds: false, includeFullNames: false);
        var getCustomersQueryRequestFaker = GetCustomersQueryRequestFaker.GenerateFromCustomers(customersFaker, includeCustomerIds: false, includeRegionIds: false, includeFullNames: false);
        
        var service = GetCustomersQueryStub.Create();
        service.CustomerRepositoryMock.GetCustomersQueryReturnCustomers(getCustomersQueryRequestFaker, customersFaker.ToArray());
        
        //Act
        var response = await service.Handle(getCustomersQueryRequestFaker, CancellationToken.None);
    
        //Assert
        service.VerifyAll();
        service.VerifyNoOtherCalls();
        response.Customers.Should().BeEquivalentTo(customersFaker.ToArray().ToBll());
    }
}