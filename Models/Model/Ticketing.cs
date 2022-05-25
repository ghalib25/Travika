using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class Ticketing
    {
        public Ticketing()
        {
            DetailsTicketings = new HashSet<DetailsTicketing>();
        }

        public int Id { get; set; }
        public int MerchantId { get; set; }
        public string Category { get; set; } = null!;
        public string Origin { get; set; } = null!;
        public string Destination { get; set; } = null!;
        public DateTime? Departure { get; set; }
        public DateTime? Arrival { get; set; }
        public int Price { get; set; }
        public int Seat { get; set; }

        public virtual MerchantProfile Merchant { get; set; } = null!;
        public virtual ICollection<DetailsTicketing> DetailsTicketings { get; set; }
    }
}
