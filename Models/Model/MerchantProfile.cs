using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class MerchantProfile
    {
        public MerchantProfile()
        {
            Hotels = new HashSet<Hotel>();
            Ticketings = new HashSet<Ticketing>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string CompanyName { get; set; } = null!;

        public virtual User User { get; set; } = null!;
        public virtual ICollection<Hotel> Hotels { get; set; }
        public virtual ICollection<Ticketing> Ticketings { get; set; }
    }
}
