using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class DetailsHotel
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int HotelId { get; set; }
        public int Quantity { get; set; }
        public DateTime Created { get; set; }

        public virtual Hotel Hotel { get; set; } = null!;
        public virtual Transaction Transaction { get; set; } = null!;
    }
}
