using System;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;

namespace Helpdesk.DataAccess
{
    public class HelpdeskUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
