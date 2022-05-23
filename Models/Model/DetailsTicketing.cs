using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class DetailsTicketing
    {
        public DetailsTicketing()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public int TicketingId { get; set; }
        public int Quantity { get; set; }

        public virtual Ticketing Ticketing { get; set; } = null!;
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
