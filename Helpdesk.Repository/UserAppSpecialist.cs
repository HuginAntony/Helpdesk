namespace Helpdesk.Repository
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserAppSpecialist")]
    public partial class UserAppSpecialist
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AppSpecialistId { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual AppSpecialist AppSpecialist { get; set; }

        public virtual User User { get; set; }
    }
}
