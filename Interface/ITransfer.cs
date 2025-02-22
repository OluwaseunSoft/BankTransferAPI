using FCMBBankTransaction.Model;

namespace FCMBBankTransaction.Interface
{
    public interface ITransfer
    {
        Task<TransferResponse> DoTransfer(TransferRequest request);
    }
}
