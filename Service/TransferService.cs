﻿using BankTransactionAPI.Interface;
using BankTransactionAPI.Model;
using System;

namespace BankTransactionAPI.Service
{
    public class TransferService : ITransfer
    {
        private readonly IAccount _account;
        private readonly ICustomer _customer;
        private readonly ITransaction _transaction;
        private readonly IRetailCustomer _retailCustomer;
        public TransferService(ICustomer customer, IAccount account, ITransaction transaction, IRetailCustomer retailCustomer)
        {
            _account = account;
            _customer = customer;
            _transaction = transaction;
            _retailCustomer = retailCustomer;
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

                var customer = await _customer.GetCustomer(account.CustomerId.ToString());
                if (customer.CustomerType == "BUSINESS")
                {
                    var customerDiscount = await BusinessCustomerDiscount(account.CustomerId, request.Amount, request.SourceAccount, customer.DateCreated);
                    transactionData.DiscountedAmount = customerDiscount.DiscountedAmount;
                    transactionData.Rate = customerDiscount.Rate;
                }
                else if (customer.CustomerType == "RETAIL")
                {
                    var customerDiscount = await _retailCustomer.RetailCustomerDiscount(account.AccountNumber, request.Amount, customer.DateCreated);
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

        private async Task<CustomerDiscountResponse> BusinessCustomerDiscount(int customerId, decimal transactionAmount, string accountNumber, DateTime customerDate)
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
                if (DateTime.Now.Year - customerDate.Year >= 4 && transactionCount < 3)
                {
                    await CustomerLoyalty(transactionAmount, result);
                    return result;
                }

                if (accountCount > 1 && transactionCount >= 3 && transactionAmount > 150000)
                {
                    result.Rate = 7;
                    result.DiscountedAmount = await GetDiscountedAmount(result.Rate, transactionAmount);
                    return result;
                }

                result.Rate = 0;
                result.DiscountedAmount = 0;
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new CustomerDiscountResponse(); ;
            }

        }
        public async Task CustomerLoyalty(decimal transactionAmount, CustomerDiscountResponse result)
        {
            result.Rate = 10;
            result.DiscountedAmount = await GetDiscountedAmount(result.Rate, transactionAmount);
        }     

        public async Task<decimal> GetDiscountedAmount(decimal rate, decimal amount)
        {
            return (rate / 100) * amount;
        }

        public async Task<int> NumberOfTransactionWithinAMonth(string accountNumber)
        {
            int transactionCount = 0;
            var transactions = await _transaction.GetTransactionData(accountNumber);
            for (int i = 0; i < transactions.ToList().Count; i++)
            {
                foreach (var transaction in transactions)
                {
                    var sameMonth = transactions.ToList()[i].TransactionDate.Month.Equals(DateTime.Now.Month);
                    if (sameMonth && transactions.ToList()[i].TransactionDate.Year == DateTime.Now.Year)
                    {
                        transactionCount++;
                    }
                }
            }
            return transactionCount;
        }
    }
}
