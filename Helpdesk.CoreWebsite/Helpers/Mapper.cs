using System.Collections.Generic;
using System.Linq;
using Helpdesk.CoreWebsite.Models;
using Helpdesk.DataAccess;

namespace Helpdesk.CoreWebsite.Helpers
{
    public static class Mapper
    {
        public static string GetPriority(this string priority)
        {
            if (priority == "High")
                return "1";
            else if (priority == "Medium")
                return "2";
            else
                return "3";
        }

        public static List<ExcelRequest> GetExcelRequests(this List<Request> allRequests)
        {
            return allRequests.Select(r => new ExcelRequest
            {
                Id = r.Id,
                Application = r.Application.Name,
                ApplicationSpecialist = r.AppSpecialist == null ? "" : r.AppSpecialist.Name,
                BookingReference = r.BookingReference,
                Brand = r.Brand.Name,
                DateCreated = r.DateCreated.ToString("dd/MM/yyyy HH:mm:ss"),
                DateResolved = r.DateResolved == null ? "" : r.DateResolved.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                Description = r.RequestMessage.First().Message,
                Developer = r.Developer == null ? "" : r.Developer.Name,
                RequestType = r.RequestType,
                Title = r.Title,
                Priority = r.Priority,
                Status = r.Status,
                TfsId = r.TfsId == null ? "" : r.TfsId.ToString(),
                User = r.User.Name
            }).ToList();
        }
    }
}
