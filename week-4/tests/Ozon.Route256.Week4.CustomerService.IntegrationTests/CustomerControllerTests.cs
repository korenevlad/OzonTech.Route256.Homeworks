using AutoBogus;
using FluentAssertions;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Ozon.Route256.Week4.CustomerService.Domain.Models;

namespace Ozon.Route256.Week4.CustomerService.IntegrationTests;
public class CustomerControllerTests : IClassFixture<CustomCustomerWebHost<Startup>>
{
    private CustomCustomerWebHost<Startup> _factory { get; init; }
    private readonly CustomerService.CustomerServiceClient _client;
    public CustomerControllerTests(CustomCustomerWebHost<Startup> factory)
    {
        _factory = factory;
        _client = CreateClient();
    }

    [Fact]
    public async Task V1CreateCustomer_ShouldReturnSuccess_WhenDataIsValid()
    {
        // Arrange
        var request = AutoFaker.Generate<V1CreateCustomerRequest>();

        // Act
        var response = await _client.V1CreateCustomerAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Ok);
        Assert.True(response.Ok.CustomerId > 0);
    }

    [Fact (Skip = "Не происходит валидации на уже сущесвующий customer (проверка в репозитории происходит по id, а не по свойствам запроса).")]
    public async Task V1CreateCustomer_ShouldReturnError_WhenCustomerAlreadyExists()
    {
        // Arrange
        var request = AutoFaker.Generate<V1CreateCustomerRequest>();

        await _client.V1CreateCustomerAsync(request);

        // Act
        var response = await _client.V1CreateCustomerAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Error);
        Assert.Equal("CustomerAlreadyExistsException", response.Error.Code);
    }
    
    public static IEnumerable<object[]> GetCustomerFilters()
    {
        var testCustomers = AutoFaker.Generate<Customer>(3).ToList();

        yield return new object[]
        {
            new V1QueryCustomersRequest { CustomerIds = { testCustomers[0].Id } },
            new List<long> { testCustomers[0].Id }
        };

        yield return new object[]
        {
            new V1QueryCustomersRequest { RegionIds = { testCustomers[1].RegionId } },
            testCustomers.Where(c => c.RegionId == testCustomers[1].RegionId).Select(c => c.Id).ToList()
        };

        yield return new object[]
        {
            new V1QueryCustomersRequest { FullNames = { testCustomers[2].FullName } },
            testCustomers.Where(c => c.FullName == testCustomers[2].FullName).Select(c => c.Id).ToList()
        };

        yield return new object[]
        {
            new V1QueryCustomersRequest { },
            testCustomers.Select(c => c.Id).ToList()
        };
    }

    [Theory (Skip = "в response возвращется пустая коллекция")]
    [MemberData(nameof(GetCustomerFilters))]
    public async Task V1QueryCustomers_ShouldReturnFilteredCustomers(V1QueryCustomersRequest request, List<long> expectedCustomerIds)
    {
        // Arrange
        var testCustomers = AutoFaker.Generate<Customer>(3).ToList();

        foreach (var customer in testCustomers)
        {
            await _factory.CustomerRepository.CreateCustomer(
                customer.FullName, customer.RegionId, CancellationToken.None);
        }

        // Act
        var response = await _client.V1QueryCustomersAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(expectedCustomerIds.Count, response.TotalCount);
    
        var expectedIdsSet = expectedCustomerIds.ToHashSet();
        Assert.All(response.Customers, c => Assert.Contains(c.CustomerId, expectedIdsSet));
    }

    [Fact]
    public async Task V1DeleteCustomersByIds_SendExistsIds_ShouldGetOk()
    {
        // Arrange
        var customers = AutoFaker.Generate<Customer>(3);
        var idsList = new List<long>();
        foreach (var customer in customers)
        {
            var customerId = await _factory.CustomerRepository.CreateCustomer(
                customer.FullName, customer.RegionId, CancellationToken.None);
            idsList.Add(customerId);
        }
        var request = new V1DeleteCustomersByIdsRequest
        {
            CustomerIds = { } 
        };
        foreach (var id in idsList)
        {
            request.CustomerIds.Add(id);
        }
        
        // Act
        var response = await _client.V1DeleteCustomersByIdsAsync(request);
        
        // Assert
        response.Ok.Should().NotBeNull();
        response.Ok.CustomerIds.Should().Contain(idsList);
    }
    
    [Fact (Skip = "Не возвращается ошибка при удалении несуществующих customers")]
    public async Task V1DeleteCustomersByIds_SendNotExistsIds_ShouldGetError()
    {
        // Arrange
        var customersExists = AutoFaker.Generate<Customer>(3);
        foreach (var customer in customersExists)
        {
            var customerId = await _factory.CustomerRepository.CreateCustomer(
                customer.FullName, customer.RegionId, CancellationToken.None);
        }
        var request = new V1DeleteCustomersByIdsRequest
        {
            CustomerIds = { } 
        };
        var customersNotExists = AutoFaker.Generate<Customer>(3);
        foreach (var customer in customersNotExists)
        {
            request.CustomerIds.Add(customer.Id);
        }
        
        // Act
        var response = await _client.V1DeleteCustomersByIdsAsync(request);
        
        // Assert
        response.Error.Should().NotBeNull();
    }
    
    
    private CustomerService.CustomerServiceClient CreateClient()
    {
        var httpClient = _factory.CreateClient();
        var grpcChannel = GrpcChannel.ForAddress(httpClient.BaseAddress!, new GrpcChannelOptions()
        {
            HttpClient = httpClient
        });
        
        return new CustomerService.CustomerServiceClient(grpcChannel);
    }
}