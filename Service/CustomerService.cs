using Dapper;
using FCMBBankTransaction.Interface;
using FCMBBankTransaction.Model;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace FCMBBankTransaction.Service
{
    public class CustomerService : ICustomer
    {
        private readonly IConfiguration _configuration;       
        public CustomerService(IConfiguration configuration)
        {
            _configuration = configuration;
        }       

        public async Task<CustomerDto> GetCustomer(string customerId)
        {
            try
            {
                using (var con = new SqlConnection(_configuration.GetConnectionString("DbConnection")))
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
