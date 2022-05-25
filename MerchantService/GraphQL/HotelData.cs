namespace MerchantService.GraphQL
{
    public class HotelData
    {
        public int MerchantId { get; set; }
        public string HotelName { get; set; } 
        public string Address { get; set; } 
        public string City { get; set; } 
        public int Price { get; set; }
        public string Status { get; set; }
        public List<HotelDetailData> Details { get; set; }
    }
}
