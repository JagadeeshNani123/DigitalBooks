using System;
using System.Collections.Generic;

namespace DigitalBooksWebAPI.Models
{
    public partial class RoleMaster
    {
        public RoleMaster()
        {
            Users = new HashSet<User>();
        }

        public string RoleName { get; set; } = null!;
        public int RoleId { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
