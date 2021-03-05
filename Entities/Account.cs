using System;
using System.Collections.Generic;

#nullable disable

namespace Hero_Project.Entities
{
    public partial class Account
    {
        public int AccountId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime Created { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
    }
}
