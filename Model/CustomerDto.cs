namespace BankTransactionAPI.Model
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerType { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
