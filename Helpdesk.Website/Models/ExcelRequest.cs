using System;

namespace Helpdesk.Website.Controllers
{
    public class ExcelRequest
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string BookingReference { get; set; }

        public string Priority { get; set; }

        public string Status { get; set; }

        public string RequestType { get; set; }

        public string Application { get; set; }

        public string Brand { get; set; }

        public string ApplicationSpecialist { get; set; }

        public string Developer { get; set; }

        public string User { get; set; }

        public string DateCreated { get; set; }

        public string DateResolved { get; set; }

        public string TfsId { get; set; }
    }
}