using BankTransactionAPI.Interface;
using BankTransactionAPI.Model;

namespace BankTransactionAPI.Service
{
    public class Utility : IUtility
    {
        private readonly ITransaction _transaction;
        public Utility(ITransaction transaction)
        {
            _transaction = transaction;
        }
        public async Task CustomerLoyalty(decimal transactionAmount, CustomerDiscountResponse result)
        {
            result.Rate = 10;
            result.DiscountedAmount = await GetDiscountedAmount(result.Rate, transactionAmount);
        }

        public async Task<decimal> GetDiscountedAmount(decimal rate, decimal amount)
        {
            return (rate / 100) * amount;
        }

        public async Task<int> NumberOfTransactionWithinAMonth(string accountNumber)
        {
            int transactionCount = 0;
            var transactions = await _transaction.GetTransactionData(accountNumber);
            for (int i = 0; i < transactions.ToList().Count; i++)
            {
                foreach (var transaction in transactions)
                {
                    var sameMonth = transactions.ToList()[i].TransactionDate.Month.Equals(DateTime.Now.Month);
                    if (sameMonth && transactions.ToList()[i].TransactionDate.Year == DateTime.Now.Year)
                    {
                        transactionCount++;
                    }
                }
            }
            return transactionCount;
        }
    }
}
