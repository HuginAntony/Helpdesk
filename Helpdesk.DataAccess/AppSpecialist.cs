using System;
using System.Collections.Generic;

namespace Helpdesk.DataAccess
{
    public partial class AppSpecialist
    {
        public AppSpecialist()
        {
            Request = new HashSet<Request>();
            UserAppSpecialist = new HashSet<UserAppSpecialist>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public bool? Active { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual ICollection<Request> Request { get; set; }
        public virtual ICollection<UserAppSpecialist> UserAppSpecialist { get; set; }
    }
}
