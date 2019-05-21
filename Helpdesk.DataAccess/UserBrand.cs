using System;
using System.Collections.Generic;

namespace Helpdesk.DataAccess
{
    public partial class UserBrand
    {
        public int UserId { get; set; }
        public short BrandId { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual User User { get; set; }
    }
}
