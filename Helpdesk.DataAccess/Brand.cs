using System;
using System.Collections.Generic;

namespace Helpdesk.DataAccess
{
    public partial class Brand
    {
        public Brand()
        {
            Request = new HashSet<Request>();
            UserBrand = new HashSet<UserBrand>();
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual ICollection<Request> Request { get; set; }
        public virtual ICollection<UserBrand> UserBrand { get; set; }
    }
}
