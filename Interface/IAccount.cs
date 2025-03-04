using BankTransactionAPI.Model;

namespace BankTransactionAPI.Interface
{
    public interface IAccount
    {
        Task<AccountDto> GetAccount(string accountNumber);
        Task<List<AccountDto>> GetAccounts(string customerId);
    }
}
