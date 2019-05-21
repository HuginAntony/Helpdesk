using System;
using System.Collections.Generic;

namespace Helpdesk.DataAccess
{
    public partial class RequestMessage
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public string Message { get; set; }
        public DateTime DateCreated { get; set; }
        public string AttachmentFilename { get; set; }
        public byte[] AttachmentData { get; set; }
        public int UserId { get; set; }

        public virtual Request Request { get; set; }
        public virtual User User { get; set; }
    }
}
