using Moq;
using Ozon.Route256.Week4.CustomerService.DAL.Repositories;
using Ozon.Route256.Week4.CustomerService.Domain.Services.GetCustomers;

namespace Ozon.Route256.Week4.CustomerService.UnitTests.Stubs;
public class GetCustomersQueryStub : GetCustomersQueryHandler
{
    public Mock<ICustomerRepository> CustomerRepositoryMock;

    public GetCustomersQueryStub(Mock<ICustomerRepository> customerRepositoryMock) : base(customerRepositoryMock.Object)
    {
        CustomerRepositoryMock = customerRepositoryMock;
    }
    
    public static GetCustomersQueryStub Create()
    {
        return new GetCustomersQueryStub(new Mock<ICustomerRepository>());
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