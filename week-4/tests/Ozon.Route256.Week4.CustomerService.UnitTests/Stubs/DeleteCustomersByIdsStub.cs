using Moq;
using Ozon.Route256.Week4.CustomerService.DAL.Repositories;
using Ozon.Route256.Week4.CustomerService.Domain.Services.DeleteCustomersByIds;

namespace Ozon.Route256.Week4.CustomerService.UnitTests.Stubs;

public class DeleteCustomersByIdsStub : DeleteCustomersByIdsCommandHandler
{
    public Mock<ICustomerRepository> CustomerRepositoryMock;
    
    public DeleteCustomersByIdsStub(Mock<ICustomerRepository> customerRepositoryMock) : base(customerRepositoryMock.Object)
    {
        CustomerRepositoryMock = customerRepositoryMock;
    }
    
    public static DeleteCustomersByIdsStub Create()
    {
        return new DeleteCustomersByIdsStub(new Mock<ICustomerRepository>());
    }

    public void VerifyAll()
    {
        CustomerRepositoryMock.VerifyAll();
    }

    public void VerifyNoOtherCalls()
    {
        CustomerRepositoryMock.VerifyNoOtherCalls();
    }
    
    
}