using System;
using System.Collections.Generic;

namespace Helpdesk.DataAccess
{
    public partial class Role
    {
        public Role()
        {
            UserRole = new HashSet<UserRole>();
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
