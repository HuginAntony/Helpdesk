using System;
using System.Collections.Generic;

namespace Helpdesk.DataAccess
{
    public partial class Developer
    {
        public Developer()
        {
            Request = new HashSet<Request>();
            UserDeveloper = new HashSet<UserDeveloper>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public bool? Active { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual ICollection<Request> Request { get; set; }
        public virtual ICollection<UserDeveloper> UserDeveloper { get; set; }
    }
}
