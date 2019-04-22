using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Helpdesk.Core;
using Helpdesk.Core.Helpers;
using Helpdesk.Repository;
using Helpdesk.Repository.CustomModels;

namespace Helpdesk.Website.Controllers
{
    [Auth]
    [IsLoggedIn]
    public class ReportController : Controller
    {
        public HelpdeskRepository Repository = new HelpdeskRepository();

        public ActionResult Index()
        {
            PopulateDropdowns();
            return View();
        }

        public void DownloadReport(string reportName, string startDate, string endDate, string appfilter)
        {
            var path = Server.MapPath("~/") + @"\Reports\";
            //var path = ConfigurationManager.AppSettings["ZipPath"];
            string fileName = reportName + Guid.NewGuid() + ".xlsx";

            var report = Repository.GetReportByApplication(reportName, DateTime.Parse(startDate), DateTime.Parse(endDate), appfilter);
            report.ToExcel(path + fileName, reportName, false);

            if (!string.IsNullOrEmpty(fileName))
            {
                CompressionHelper compress = new CompressionHelper();


                var zippedFile = compress.ZipFileToByteArray(path + fileName, path + fileName.Replace("xlsx", "zip"));

                Response.AddHeader("content-disposition", "attachment;filename=" + fileName.Replace("xlsx", "zip"));
                Response.ContentType = "application/zip";
                Response.BinaryWrite(zippedFile);
                Response.Flush();
                Response.End();
            }
        }

        public JsonResult RequestTypeReport(string reportName, string startDate, string endDate, string appfilter)
        {
            var report = Repository.GetReportByApplication(reportName, DateTime.Parse(startDate), DateTime.Parse(endDate), appfilter);
            return Json(report, JsonRequestBehavior.AllowGet);
        }

        private void PopulateDropdowns()
        {
            var applications = Repository.GetApplications();
            var applicationList =
                applications.Select(f => new SelectListItem() { Text = f.Name, Value = f.Id.ToString() }).ToList();

            ViewData["Applications"] = applicationList;
        }
    }
}