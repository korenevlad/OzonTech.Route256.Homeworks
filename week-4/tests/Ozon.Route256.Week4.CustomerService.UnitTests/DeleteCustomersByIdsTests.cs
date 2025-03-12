using Ozon.Route256.Week4.CustomerService.UnitTests.Extensions;
using Ozon.Route256.Week4.CustomerService.UnitTests.Fakers;
using Ozon.Route256.Week4.CustomerService.UnitTests.Stubs;
using Xunit;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;

namespace Ozon.Route256.Week4.CustomerService.UnitTests;
public class DeleteCustomersByIdsTests
{
    [Fact]
    public async Task Handle_SendRequestWithFiltersCustomerIds_SuccessDeleted()
    {
        //Arrange
        var customersFaker = CustomerDbRecordFaker.GenerateListForDeleteCustomersByIds();
        var deleteCustomersByIdsCommandRequestFaker = DeleteCustomersByIdsCommandRequestFaker.GenerateFromCustomers(customersFaker);

        var service = DeleteCustomersByIdsStub.Create();
        service.CustomerRepositoryMock.DeleteCustomersByIdsReturnIds(deleteCustomersByIdsCommandRequestFaker);

        //Act
        var response = await service.Handle(deleteCustomersByIdsCommandRequestFaker, CancellationToken.None);
        
        //Assert
        service.VerifyAll();
        service.VerifyNoOtherCalls();
        // BUG: те id, которые в запросе попадают в бизнес-слой, а затем в репозиторий, возвращаются сразу из бизнес-слоя, а не из метода репозитория.
        response.CustomerIds.Should().BeEquivalentTo(deleteCustomersByIdsCommandRequestFaker.CustomerIds);
    }
}