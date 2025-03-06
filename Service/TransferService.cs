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
        private readonly IRetailCustomer _retailCustomer;
        private readonly IBusinessCustomer _businessCustomer;
        public TransferService(ICustomer customer, IAccount account, ITransaction transaction, IRetailCustomer retailCustomer, IBusinessCustomer businessCustomer)
        {
            _account = account;
            _customer = customer;
            _transaction = transaction;
            _retailCustomer = retailCustomer;
            _businessCustomer = businessCustomer;
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
                    var customerDiscount = await _businessCustomer.BusinessCustomerDiscount(account.CustomerId, request.Amount, request.SourceAccount, customer.DateCreated);
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
    }
}
