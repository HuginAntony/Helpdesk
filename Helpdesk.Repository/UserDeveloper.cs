namespace Helpdesk.Repository
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserDeveloper")]
    public partial class UserDeveloper
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DeveloperId { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual Developer Developer { get; set; }

        public virtual User User { get; set; }
    }
}
