using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class Payment
    {
        public int Id { get; set; }
        public string PaymentMethod { get; set; } = null!;
    }
}
