using BankTransactionAPI.Interface;
using BankTransactionAPI.Model;

namespace BankTransactionAPI.Service
{
    public class TransferService : ITransfer
    {
        private readonly IAccount _account;
        private readonly ICustomer _customer;
        private readonly ITransaction _transaction;
        public TransferService(ICustomer customer, IAccount account, ITransaction transaction)
        {
            _account = account;
            _customer = customer;
            _transaction = transaction;
        }

        public async Task<TransferResponse> DoTransfer(TransferRequest request)
        {
            var result = new TransferResponse();
            try
            {
                var customer = await _customer.GetCustomer(request.SourceAccount);
                if(customer.CustomerName != null)
                {
                    result = await _transaction.SaveTransactionData(request);
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return result;
            }
        }
    }
}
