namespace Helpdesk.Repository
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Attachment")]
    public partial class Attachment
    {
        public int Id { get; set; }

        public int RequestId { get; set; }

        [Required]
        [StringLength(50)]
        public string Filename { get; set; }

        [Required]
        public byte[] Data { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual Request Request { get; set; }
    }
}
