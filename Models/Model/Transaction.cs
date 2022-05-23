using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string VirtualAccount { get; set; } = null!;
        public int? DetailsTicketingId { get; set; }
        public int? DetailsHotelId { get; set; }
        public int PaymentId { get; set; }
        public int Total { get; set; }
        public string Status { get; set; } = null!;

        public virtual DetailsHotel? DetailsHotel { get; set; }
        public virtual DetailsTicketing? DetailsTicketing { get; set; }
        public virtual Payment Payment { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
