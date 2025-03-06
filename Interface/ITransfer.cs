using BankTransactionAPI.Model;

namespace BankTransactionAPI.Interface
{
    public interface ITransfer
    {
        Task<TransferResponse> DoTransfer(TransferRequest request);
        Task CustomerLoyalty(decimal transactionAmount, CustomerDiscountResponse result);
        Task<decimal> GetDiscountedAmount(decimal rate, decimal amount);
        Task<int> NumberOfTransactionWithinAMonth(string accountNumber);
    }
}
