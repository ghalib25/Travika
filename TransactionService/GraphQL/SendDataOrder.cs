namespace TransactionService.GraphQL
{
    public class SendDataOrder
    {
        public int TransactionId { get; set;}
        public string Virtualaccount { get; set;}
        public String Bills { get; set;}
        public string PaymentStatus { get; set;}
    }
}
