namespace BankTransactionAPI.Model
{
    public class TransferResponse
    {
        public string SourceAccount { get; set; }
        public string DestinationAccount { get; set; }
        public decimal Amount { get; set; }
        public string ResponseDescription { get; set; }
        public string ResponseCode { get; set; }
        public DateTime TransactionTime { get; set; }

    }
}
