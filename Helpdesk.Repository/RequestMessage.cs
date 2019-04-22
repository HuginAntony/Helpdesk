namespace Helpdesk.Repository
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RequestMessage")]
    public partial class RequestMessage
    {
        public int Id { get; set; }

        public int RequestId { get; set; }

        [Required]
        [StringLength(1500)]
        public string Message { get; set; }

        public DateTime DateCreated { get; set; }

        [StringLength(50)]
        public string AttachmentFilename { get; set; }

        public byte[] AttachmentData { get; set; }

        public int UserId { get; set; }

        public virtual Request Request { get; set; }

        public virtual User User { get; set; }
    }
}
