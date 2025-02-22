using BankTransactionAPI.Interface;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BankTransactionAPI.Model
{
    public class DapperDbConnection : IDapperDbConnection
    {
        
        private readonly string _connection;

        public DapperDbConnection(IConfiguration configuration)
        {           
            _connection = configuration.GetConnectionString("DbConnection");
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connection);
        }
    }
}
