using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class Payment
    {
        public Payment()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string PaymentMethod { get; set; } = null!;

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
