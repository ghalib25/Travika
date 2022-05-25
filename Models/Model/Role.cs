﻿using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class Role
    {
        public Role()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string Role1 { get; set; } = null!;

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
