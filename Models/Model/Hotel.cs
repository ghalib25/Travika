using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class Hotel
    {
        public Hotel()
        {
            DetailsHotels = new HashSet<DetailsHotel>();
        }

        public int Id { get; set; }
        public int MerchantId { get; set; }
        public string HotelName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public int Price { get; set; }
        public int Room { get; set; }

        public virtual MerchantProfile Merchant { get; set; } = null!;
        public virtual ICollection<DetailsHotel> DetailsHotels { get; set; }
    }
}
