namespace Helpdesk.Repository
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Request")]
    public partial class Request
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Request()
        {
            RequestMessages = new HashSet<RequestMessage>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(6)]
        public string BookingReference { get; set; }

        [Required]
        [StringLength(6)]
        public string Priority { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        [Required]
        [StringLength(15)]
        public string RequestType { get; set; }

        public short ApplicationId { get; set; }

        public short BrandId { get; set; }

        public int? AppSpecialistId { get; set; }

        public int? DeveloperId { get; set; }

        public int UserId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateResolved { get; set; }

        public int? TfsId { get; set; }

        public virtual Application Application { get; set; }

        public virtual AppSpecialist AppSpecialist { get; set; }

        public virtual Brand Brand { get; set; }

        public virtual Developer Developer { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequestMessage> RequestMessages { get; set; }
    }
}
