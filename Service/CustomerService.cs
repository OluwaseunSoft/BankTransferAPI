using Dapper;
using BankTransactionAPI.Interface;
using BankTransactionAPI.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace BankTransactionAPI.Service
{
    public class CustomerService : ICustomer
    {
        private readonly DapperDbConnection _dbContext;
        public CustomerService(DapperDbConnection dbContext)
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
