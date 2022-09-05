using System;
using System.Collections.Generic;

namespace DigitalBooksWebAPI.Models
{
    public partial class User
    {
        public User()
        {
            Books = new HashSet<Book>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string EmailId { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }
        public bool Active { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public virtual RoleMaster Role { get; set; } = null!;
        public virtual ICollection<Book> Books { get; set; }
    }
}
