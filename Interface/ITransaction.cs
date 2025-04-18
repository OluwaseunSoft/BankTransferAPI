﻿using BankTransactionAPI.Model;

namespace BankTransactionAPI.Interface
{
    public interface ITransaction
    {
        Task<IEnumerable<TransactionDataDto>> GetTransactionData(string accountNumber);
        Task<int> SaveTransactionData(TransactionDataDto request);
    }
}
