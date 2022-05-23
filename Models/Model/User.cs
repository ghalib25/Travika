using System;
using System.Collections.Generic;

namespace Model.Model
{
    public partial class User
    {
        public User()
        {
            CustomerProfiles = new HashSet<CustomerProfile>();
            MerchantProfiles = new HashSet<MerchantProfile>();
            Transactions = new HashSet<Transaction>();
            UserRoles = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual ICollection<CustomerProfile> CustomerProfiles { get; set; }
        public virtual ICollection<MerchantProfile> MerchantProfiles { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
