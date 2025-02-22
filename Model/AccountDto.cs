namespace BankTransactionAPI.Model
{
    public class AccountDto
    {
        public int AccountId { get; set; }
        public string AccountNumber { get; set; }
        public int CustomerId { get; set; }
        public decimal AccountBalance { get; set; }
        public DateTime AccountOpenDate { get; set; }
    }
}
