using System;
using System.Collections.Generic;

namespace Helpdesk.DataAccess
{
    public partial class UserDeveloper
    {
        public int UserId { get; set; }
        public int DeveloperId { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual Developer Developer { get; set; }
        public virtual User User { get; set; }
    }
}
