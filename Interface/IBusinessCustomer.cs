using BankTransactionAPI.Model;

namespace BankTransactionAPI.Interface
{
    public interface IBusinessCustomer
    {
        Task<CustomerDiscountResponse> BusinessCustomerDiscount(int customerId, decimal transactionAmount, string accountNumber, DateTime customerDate);
    }
}
