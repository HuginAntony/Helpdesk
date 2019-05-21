using System;
using System.Collections.Generic;

namespace Helpdesk.DataAccess
{
    public partial class UserAppSpecialist
    {
        public int UserId { get; set; }
        public int AppSpecialistId { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual AppSpecialist AppSpecialist { get; set; }
        public virtual User User { get; set; }
    }
}
