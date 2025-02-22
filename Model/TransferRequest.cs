namespace FCMBBankTransaction.Model
{
    public class TransferRequest
    {
        public string SourceAccount { get; set; }
        public string DestinationAccount { get; set; }
        public decimal Amount { get; set; }
    }
}
