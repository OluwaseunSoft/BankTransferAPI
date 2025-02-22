using BankTransactionAPI.Model;

namespace BankTransactionAPI.Interface
{
    public interface ITransfer
    {
        Task<TransferResponse> DoTransfer(TransferRequest request);
    }
}
