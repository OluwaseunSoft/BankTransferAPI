using BankTransactionAPI.Interface;
using BankTransactionAPI.Model;

namespace BankTransactionAPI.Service
{
    public class BusinessCustomer : IBusinessCustomer
    {
        private readonly IAccount _account;
        private readonly IUtility _utility;
        public BusinessCustomer(IAccount account, IUtility utility)
        {
            _account = account;
            _utility = utility;
        }
        public async Task<CustomerDiscountResponse> BusinessCustomerDiscount(int customerId, decimal transactionAmount, string accountNumber, DateTime customerDate)
        {
            var result = new CustomerDiscountResponse();
            int accountCount = 0;
            try
            {
                var customerAccounts = await _account.GetAccounts(customerId.ToString());
                if (customerAccounts == null) throw new Exception("Invalid Customer");

                for (int i = 0; i < customerAccounts.Count; i++)
                {
                    foreach (var account in customerAccounts)
                    {
                        var sameYear = customerAccounts[i].AccountOpenDate.Year.Equals(account.AccountOpenDate.Year);
                        if (sameYear && customerAccounts[i].AccountNumber != account.AccountNumber)
                        {
                            accountCount++;
                        }
                    }
                }
                var transactionCount = await _utility.NumberOfTransactionWithinAMonth(accountNumber);
                if (DateTime.Now.Year - customerDate.Year >= 4 && transactionCount < 3)
                {
                    await _utility.CustomerLoyalty(transactionAmount, result);
                    return result;
                }

                if (accountCount > 1 && transactionCount >= 3 && transactionAmount > 150000)
                {
                    result.Rate = 7;
                    result.DiscountedAmount = await _utility.GetDiscountedAmount(result.Rate, transactionAmount);
                    return result;
                }

                result.Rate = 0;
                result.DiscountedAmount = 0;
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new CustomerDiscountResponse(); ;
            }

        }
    }
}

