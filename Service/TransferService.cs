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
                if (customerType.CustomerType == "BUSINESS")
                {
                    var customerDiscount = await BusinessCustomerDiscount(account.CustomerId);
                    transactionData.DiscountedAmount = customerDiscount.DiscountedAmount;
                    transactionData.Rate = customerDiscount.Rate;
                }
                else if (customerType.CustomerType == "RETAIL")
                {
                    var customerDiscount = await RetailCustomerDiscount(account.AccountNumber);
                    transactionData.DiscountedAmount = customerDiscount.DiscountedAmount;
                    transactionData.Rate = customerDiscount.Rate;
                }

                transactionData.TransactionDate = DateOnly.FromDateTime(DateTime.Now);
                transactionData.Amount = request.Amount;
                transactionData.AccountNumber = request.SourceAccount;
                var doTransfer = await _transaction.SaveTransactionData(transactionData);

                if (doTransfer > 0)
                {
                    result.ResponseDescription = "Successful";
                    result.ResponseCode = "00";
                    result.Amount = request.Amount;
                    result.DestinationAccount = request.DestinationAccount;
                    result.SourceAccount = request.SourceAccount;
                    result.TransactionTime = DateTime.Now;
                }

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

        private async Task<CustomerDiscountResponse> BusinessCustomerDiscount(int customerId)
        {
            var result = new CustomerDiscountResponse();
            int accountCount = 0;
            var customerAccounts = await _account.GetAccounts(customerId.ToString());
            if (customerAccounts != null)
            {
                var accountss = customerAccounts.Where(x=>x.AccountOpenDate > )
            }
            result.DiscountedAmount = 0;
            result.Rate = 0;
            return result;
        }

        private async Task<CustomerDiscountResponse> RetailCustomerDiscount(string accountNumber)
        {
            var result = new CustomerDiscountResponse();
            result.DiscountedAmount = 0;
            result.Rate = 0;
            return result;
        }
    }
}
