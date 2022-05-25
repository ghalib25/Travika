using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class MerchantProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CompanyName { get; set; } = null!;
    }
}
