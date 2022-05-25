namespace MerchantService.GraphQL
{
    public partial class TicketingData
    {
        public int? Id { get; set; }
        public int MerchantId { get; set; }
        public string Category { get; set; } 
        public string Origin { get; set; } 
        public string Destination { get; set; }
        public int Price { get; set; }
        public List<TicketingDetailData> Details { get; set; }
    }
}
