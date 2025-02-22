using Dapper;
using FCMBBankTransaction.Interface;
using FCMBBankTransaction.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using BankTransactionAPI.Model;

namespace FCMBBankTransaction.Service
{
    public class AccountService : IAccount
    {
        private readonly BankTransactionDbContext _dbContext;
        public AccountService(BankTransactionDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<AccountDto> GetAccount(string accountNumber)
        {
            try
            {
                using (var con = _dbContext.CreateConnection())
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
