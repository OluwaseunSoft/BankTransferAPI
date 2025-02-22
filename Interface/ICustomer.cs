using BankTransactionAPI.Model;

namespace BankTransactionAPI.Interface
{
    public interface ICustomer
    {
        Task<CustomerDto> GetCustomer(string customerId);
    }
}
