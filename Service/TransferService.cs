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
                    var customerDiscount = await BusinessCustomerDiscount(account.CustomerId, request.Amount, request.SourceAccount);
                    transactionData.DiscountedAmount = customerDiscount.DiscountedAmount;
                    transactionData.Rate = customerDiscount.Rate;
                }
                else if (customerType.CustomerType == "RETAIL")
                {
                    var customerDiscount = await RetailCustomerDiscount(account.AccountNumber);
                    transactionData.DiscountedAmount = customerDiscount.DiscountedAmount;
                    transactionData.Rate = customerDiscount.Rate;
                }

                transactionData.TransactionDate = DateTime.Now;
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

        private async Task<CustomerDiscountResponse> BusinessCustomerDiscount(int customerId, decimal transactionAmount, string accountNumber)
        {
            var result = new CustomerDiscountResponse();
            int accountCount = 0;
            try
            {
                var customerAccounts = await _account.GetAccounts(customerId.ToString());
                if (customerAccounts == null) throw new Exception("Invalid Customer");

                for (int i = 0; i < customerAccounts.Count; i++)
                {
                    foreach (var account in customerAccounts)
                    {
                        var sameYear = customerAccounts[i].AccountOpenDate.Year.Equals(account.AccountOpenDate.Year);
                        if (sameYear && customerAccounts[i].AccountNumber != account.AccountNumber)
                        {
                            accountCount++;
                        }
                    }
                }
                var transactionCount = await NumberOfTransactionWithinAMonth(accountNumber);
                if (accountCount > 1 && transactionAmount > 3)
                {
                    result.Rate = 0.07M;
                    result.DiscountedAmount = await GetDiscountedAmount(result.Rate, transactionAmount);
                    
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new CustomerDiscountResponse(); ;
            }

        }

        private async Task<CustomerDiscountResponse> RetailCustomerDiscount(string accountNumber)
        {
            var result = new CustomerDiscountResponse();
            result.DiscountedAmount = 0;
            result.Rate = 0;
            return result;
        }

        private async Task<decimal> GetDiscountedAmount(decimal rate, decimal amount)
        {
            return amount * rate;
        }

        private async Task<int> NumberOfTransactionWithinAMonth(string accountNumber)
        {
            int transactionCount = 0;
            var transactions = await _transaction.GetTransactionData(accountNumber);
            for (int i = 0; i < transactions.ToList().Count; i++)
            {
                foreach (var transaction in transactions)
                {
                    var sameMonth = transactions.ToList()[i].TransactionDate.Month.Equals(transaction.TransactionDate.Month);
                    if (sameMonth && transactions.ToList()[i].TransactionDate.Year == transaction.TransactionDate.Year)
                    {
                        transactionCount++;
                    }
                }
            }
            return transactionCount;
        }
    }
}
