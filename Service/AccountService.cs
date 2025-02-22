using Dapper;
using FCMBBankTransaction.Interface;
using FCMBBankTransaction.Model;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace FCMBBankTransaction.Service
{
    public class AccountService : IAccount
    {
        private readonly IConfiguration _configuration;
        public AccountService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<AccountDto> GetAccount(string accountNumber)
        {
            try
            {
                using (var con = new SqlConnection(_configuration.GetConnectionString("DbConnection")))
                {
                    con.Open();
                    string query = "SELECT AccountId, AccountNumber, CustomerId, AccountBalance, AccountOpenDate FROM AccountData WHERE AccountNumber = @accountNumber";
                    var result = await con.QueryFirstOrDefaultAsync<AccountDto>(query, new { accountNumber = accountNumber });
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new AccountDto();
            }
        }
    }
}
