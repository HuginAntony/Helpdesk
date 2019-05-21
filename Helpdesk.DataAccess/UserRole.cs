using System;
using System.Collections.Generic;

namespace Helpdesk.DataAccess
{
    public partial class UserRole
    {
        public int UserId { get; set; }
        public short RoleId { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
