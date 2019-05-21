using System;
using System.Collections.Generic;

namespace Helpdesk.DataAccess
{
    public partial class User
    {
        public User()
        {
            Request = new HashSet<Request>();
            RequestMessage = new HashSet<RequestMessage>();
            UserAppSpecialist = new HashSet<UserAppSpecialist>();
            UserBrand = new HashSet<UserBrand>();
            UserDeveloper = new HashSet<UserDeveloper>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public bool? Active { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public virtual UserRole UserRole { get; set; }
        public virtual ICollection<Request> Request { get; set; }
        public virtual ICollection<RequestMessage> RequestMessage { get; set; }
        public virtual ICollection<UserAppSpecialist> UserAppSpecialist { get; set; }
        public virtual ICollection<UserBrand> UserBrand { get; set; }
        public virtual ICollection<UserDeveloper> UserDeveloper { get; set; }
    }
}
