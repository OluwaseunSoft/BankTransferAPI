using Dapper;
using FCMBBankTransaction.Interface;
using FCMBBankTransaction.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using BankTransactionAPI.Model;

namespace FCMBBankTransaction.Service
{
    public class CustomerService : ICustomer
    {
        private readonly BankTransactionDbContext _dbContext;
        public CustomerService(BankTransactionDbContext dbContext)
        {
            _dbContext = dbContext;
        }       

        public async Task<CustomerDto> GetCustomer(string customerId)
        {
            try
            {
                using (var con = _dbContext.CreateConnection())
                {
                    con.Open();
                    string query = "SELECT CustomerId, CustomerName, CustomerType, DateCreated FROM CustomerData WHERE CustomerId = @customerId";
                    var result = await con.QueryFirstOrDefaultAsync<CustomerDto>(query, new { customerId = customerId });
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new CustomerDto();
            }
        }
    }
}
