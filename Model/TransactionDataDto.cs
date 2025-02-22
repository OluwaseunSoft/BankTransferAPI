namespace BankTransactionAPI.Model
{
    public class TransactionDataDto
    {
        public int TransactionId { get; set; }
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountedAmount { get; set; }
        public decimal Rate { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
