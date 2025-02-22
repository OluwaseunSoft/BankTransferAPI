using FCMBBankTransaction.Model;

namespace FCMBBankTransaction.Interface
{
    public interface IAccount
    {
        Task<AccountDto> GetAccount(string accountNumber);
    }
}
