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
            var transactionData = new TransactionDataDto();
            try
            {
                var account = await _account.GetAccount(request.SourceAccount);
                if (account == null || account?.AccountNumber == "")
                {
                    throw new Exception("Invalid Source Account");
                }
                var customerType = await GetCustomerType(account.CustomerId);
                if(customerType.CustomerType == "BUSINESS")
                {
                    transactionData.DiscountedAmount = await BusinessCustomerDiscount(account.CustomerId);
                }
                else if (customerType.CustomerType == "RETAIL")
                {
                    transactionData.DiscountedAmount = await RetailCustomerDiscount(account.AccountNumber);
                }
                
                transactionData.TransactionDate = DateTime.Now;
                transactionData.Rate = 0;
                transactionData.Amount = request.Amount;
                transactionData.AccountNumber = request.SourceAccount;
                transactionData.DiscountedAmount = 0;
                var doTransfer = await _transaction.SaveTransactionData(transactionData);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<CustomerDto> GetCustomerType(int customerId)
        {
            var customer = await _customer.GetCustomer(customerId.ToString());
            return customer;
        }

        private async Task<int> BusinessCustomerDiscount(int customerId)
        {
            return 0;
        }

        private async Task<int> RetailCustomerDiscount(string accountNumber)
        {
            return 0;
        }
    }
}
