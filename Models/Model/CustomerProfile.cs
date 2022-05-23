﻿using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class CustomerProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Phoine { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
