using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class Transaction
    {
        public Transaction()
        {
            DetailsHotels = new HashSet<DetailsHotel>();
            DetailsTicketings = new HashSet<DetailsTicketing>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string VirtualAccount { get; set; } = null!;
        public int PaymentId { get; set; }
        public int TotalBill { get; set; }
        public string PaymentStatus { get; set; } = null!;

        public virtual Payment Payment { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<DetailsHotel> DetailsHotels { get; set; }
        public virtual ICollection<DetailsTicketing> DetailsTicketings { get; set; }
    }
}
