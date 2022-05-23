using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class DetailsHotel
    {
        public DetailsHotel()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public int HotelId { get; set; }
        public int Quantity { get; set; }

        public virtual Hotel Hotel { get; set; } = null!;
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
