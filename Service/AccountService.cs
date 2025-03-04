using Dapper;
using BankTransactionAPI.Interface;
using BankTransactionAPI.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace BankTransactionAPI.Service
{
    public class AccountService : IAccount
    {
        private readonly IDapperDbConnection _dbContext;
        public AccountService(IDapperDbConnection dbContext)
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
        public async Task<List<AccountDto>> GetAccounts(string customerId)
        {
            try
            {
                using (var con = _dbContext.CreateConnection())
                {
                    con.Open();
                    string query = "SELECT AccountId, AccountNumber, CustomerId, AccountBalance, AccountOpenDate FROM AccountData WHERE CustomerId = @customerId";
                    var result = await con.QueryAsync<AccountDto>(query, new { customerId = customerId });
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<AccountDto>();
            }
        }
    }
}
