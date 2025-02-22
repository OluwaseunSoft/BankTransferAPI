using Dapper;
using FCMBBankTransaction.Interface;
using FCMBBankTransaction.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using BankTransactionAPI.Model;

namespace FCMBBankTransaction.Service
{
    public class TransactionService : ITransaction
    {
        private readonly BankTransactionDbContext _dbContext;

        public TransactionService(BankTransactionDbContext dbContext)
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
        public async Task<TransferResponse> SaveTransactionData(TransferRequest request)
        {
            var transactionResponse = new TransferResponse();
            try
            {
                using (var con = _dbContext.CreateConnection())
                {
                    con.Open();
                    string query = "SELECT TransactionId, AccountNumber, Amount, DiscountedAmount, Rate, TransactionDate FROM TransactionData WHERE AccountNumber = @accountNumber";
                    //var result = await con.ExecuteAsync<TransferResponse>(query, new { });
                    return transactionResponse;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return transactionResponse;
            }
        }
    }
}
