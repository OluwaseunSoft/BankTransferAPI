using BankTransactionAPI.Interface;
using BankTransactionAPI.Model;

namespace BankTransactionAPI.Service
{
    public class RetailCustomer : IRetailCustomer
    {
         
        public RetailCustomer()
        {
            
        }
        public async Task<CustomerDiscountResponse> RetailCustomerDiscount(string accountNumber, decimal transactionAmount, DateTime customerDate)
        {
            var result = new CustomerDiscountResponse();
            var transactionCount = await NumberOfTransactionWithinAMonth(accountNumber);

            if (DateTime.Now.Year - customerDate.Year >= 4 && transactionCount < 3)
            {
                await CustomerLoyalty(transactionAmount, result);
                return result;
            }

            if (transactionCount >= 3 && transactionAmount > 50000 && transactionAmount < 100000)
            {
                result.Rate = 2;
                result.DiscountedAmount = await GetDiscountedAmount(result.Rate, transactionAmount);
                return result;
            }

            result.Rate = 0;
            result.DiscountedAmount = 0;
            return result;
        }
    }
}
