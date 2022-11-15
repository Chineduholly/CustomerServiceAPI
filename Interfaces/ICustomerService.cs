namespace CustomerServiceAPI.Interfaces
{
    public interface ICustomerService
    {
        Task <ServiceResponse<Customer>> CreateCustomer(CustomerDto payload);
        Task<ServiceResponse<List<Customer>>> GetAllCustomer( );
        Task<ServiceResponse<Customer>> GetById(string CustomerId);
    }
}
