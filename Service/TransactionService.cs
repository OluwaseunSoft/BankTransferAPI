﻿using Dapper;
using BankTransactionAPI.Interface;
using BankTransactionAPI.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace BankTransactionAPI.Service
{
    public class TransactionService : ITransaction
    {
        private readonly IDapperDbConnection _dbContext;

        public TransactionService(IDapperDbConnection dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<TransactionDataDto>> GetTransactionData(string accountNumber)
        {
            var transactionData = new List<TransactionDataDto>();
            try
            {
                using (var con = _dbContext.CreateConnection())
                {
                    con.Open();
                    string query = "SELECT TransactionId, AccountNumber, Amount, DiscountedAmount, Rate, TransactionDate FROM TransactionData WHERE AccountNumber = @accountNumber";
                    var result = await con.QueryAsync<TransactionDataDto>(query, new { accountNumber = accountNumber });
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return await Task.FromResult(Enumerable.Empty<TransactionDataDto>());
            }
        }
        public async Task<int> SaveTransactionData(TransactionDataDto request)
        {            
            try
            {
                using (var con = _dbContext.CreateConnection())
                {
                    con.Open();
                    string query = "INSERT INTO TransactionData (AccountNumber, Amount, DiscountedAmount, Rate, TransactionDate) " +
                        "VALUES (@AccountNumber, @Amount, @DiscountedAmount, @Rate, @TransactionDate)";
                    var result = await con.ExecuteAsync(query, request);                   
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
    }
}
