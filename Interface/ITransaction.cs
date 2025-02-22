using FCMBBankTransaction.Model;

namespace FCMBBankTransaction.Interface
{
    public interface ITransaction
    {
        Task<IEnumerable<TransactionDataDto>> GetTransactionData(string accountNumber);
        Task<TransferResponse> SaveTransactionData(TransferRequest request);
    }
}
