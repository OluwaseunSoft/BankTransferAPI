using BankTransactionAPI.Model;

namespace BankTransactionAPI.Interface
{
    public interface IRetailCustomer
    {
        Task<CustomerDiscountResponse> RetailCustomerDiscount(string accountNumber, decimal transactionAmount, DateTime customerDate);
    }
}
