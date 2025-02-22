using Dapper;
using FCMBBankTransaction.Interface;
using FCMBBankTransaction.Model;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace FCMBBankTransaction.Service
{
    public class TransactionService : ITransaction
    {
        private readonly IConfiguration _configuration;
        public TransactionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IEnumerable<TransactionDataDto>> GetTransactionData(string accountNumber)
        {
            var transactionData = new List<TransactionDataDto>();
            try
            {
                using (var con = new SqlConnection(_configuration.GetConnectionString("DbConnection")))
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
    }
}
