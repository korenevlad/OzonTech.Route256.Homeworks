using Moq;
using Ozon.Route256.Week4.CustomerService.DAL.Repositories;
using Ozon.Route256.Week4.CustomerService.Domain.Services.CreateCustomer;

namespace Ozon.Route256.Week4.CustomerService.UnitTests.Stubs;
public class CreateCustomerStub : CreateCustomerCommandHandler
{
    public Mock<ICustomerRepository> CustomerRepositoryMock; 
    public CreateCustomerStub(Mock<ICustomerRepository> customerRepositoryMock) : base(customerRepositoryMock.Object)
    {
        CustomerRepositoryMock = customerRepositoryMock;
    }

    public static CreateCustomerStub Create()
    {
        return new CreateCustomerStub(new Mock<ICustomerRepository>());
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