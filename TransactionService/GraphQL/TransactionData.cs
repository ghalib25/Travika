namespace TransactionService.GraphQL
{
    public class TransactionData
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string VirtualAccount { get; set; } = null!;
        public int? DetailsTicketingId { get; set; }
        public int? DetailsHotelId { get; set; }
        public int PaymentId { get; set; }
        public int Total { get; set; }
        public string Status { get; set; } = null!;
    }
}
