using FCMBBankTransaction.Model;

namespace FCMBBankTransaction.Interface
{
    public interface ICustomer
    {
        Task<CustomerDto> GetCustomer(string customerId);
    }
}
