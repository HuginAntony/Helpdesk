using System;
using System.Collections.Generic;

namespace Helpdesk.DataAccess
{
    public partial class Request
    {
        public Request()
        {
            RequestMessage = new HashSet<RequestMessage>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string BookingReference { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string RequestType { get; set; }
        public short ApplicationId { get; set; }
        public short BrandId { get; set; }
        public int? AppSpecialistId { get; set; }
        public int? DeveloperId { get; set; }
        public int UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateResolved { get; set; }
        public int? TfsId { get; set; }

        public virtual AppSpecialist AppSpecialist { get; set; }
        public virtual Application Application { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual Developer Developer { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<RequestMessage> RequestMessage { get; set; }
    }
}
