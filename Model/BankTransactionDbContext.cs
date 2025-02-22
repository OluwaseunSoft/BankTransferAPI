using Microsoft.Data.SqlClient;
using System.Data;

namespace BankTransactionAPI.Model
{
    public class BankTransactionDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connection;

        public BankTransactionDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = _configuration.GetConnectionString("DbConnection");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connection);
    }
}
