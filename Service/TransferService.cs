using BankTransactionAPI.Interface;
using BankTransactionAPI.Model;
using System;

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
                var account = await _account.GetAccount(request.SourceAccount);
                if(account != null && account?.AccountNumber != "")
                {
                    var transactionData = new TransactionDataDto();
                    transactionData.TransactionDate = DateTime.Now;
                    transactionData.Rate = 0;
                    transactionData.Amount = request.Amount;
                    transactionData.AccountNumber = request.SourceAccount;
                    transactionData.DiscountedAmount = 0;
                    var doTransfer = await _transaction.SaveTransactionData(transactionData);
                }
                else
                {
                    throw new Exception("Invalid Source Account");
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
