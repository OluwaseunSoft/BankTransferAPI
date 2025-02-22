using FCMBBankTransaction.Interface;
using FCMBBankTransaction.Model;

namespace FCMBBankTransaction.Service
{
    public class TransferService : ITransfer
    {
        private readonly IAccount _account;
        private readonly ICustomer _customer;
        public TransferService(ICustomer customer, IAccount account)
        {
            _account = account;
            _customer = customer;
        }

        public async Task<TransferResponse> DoTransfer(TransferRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
