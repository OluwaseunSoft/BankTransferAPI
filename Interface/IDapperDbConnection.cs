using System.Data;

namespace BankTransactionAPI.Interface
{
    public interface IDapperDbConnection
    {
        public IDbConnection CreateConnection();
    }
}
