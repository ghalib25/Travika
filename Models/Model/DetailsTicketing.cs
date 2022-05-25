using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class DetailsTicketing
    {
        public int Id { get; set; }
        public int TranscationId { get; set; }
        public int TicketingId { get; set; }
        public int Quantity { get; set; }
        public DateTime Created { get; set; }

        public virtual Ticketing Ticketing { get; set; } = null!;
        public virtual Transaction Transcation { get; set; } = null!;
    }
}
