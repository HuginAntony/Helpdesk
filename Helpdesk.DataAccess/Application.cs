using System;
using System.Collections.Generic;

namespace Helpdesk.DataAccess
{
    public partial class Application
    {
        public Application()
        {
            Request = new HashSet<Request>();
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual ICollection<Request> Request { get; set; }
    }
}
