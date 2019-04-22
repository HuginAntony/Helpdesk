using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Helpdesk.Core;
using Helpdesk.Core.Helpers;
using Helpdesk.Repository;
using Helpdesk.Repository.CustomModels;
using Microsoft.Ajax.Utilities;

namespace Helpdesk.Website.Controllers
{
    [Auth]
    [IsLoggedIn]
    public class RequestController : Controller
    {
        public HelpdeskRepository Repository = new HelpdeskRepository();

        public ActionResult Index()
        {
            PopulateApplications();
            return View();
        }

        public ActionResult NewRequest()
        {
            PopulateApplications();
            ViewData["Priority"] = ModelConstants.Priority.Select(p => new SelectListItem() { Text = p, Value = p }).ToList();
            ViewData["RequestType"] = Repository.GetRequestTypes().Select(p => new SelectListItem() { Text = p, Value = p }).ToList();
            return View();
        }

        private void PopulateApplications()
        {
            var applications = Repository.GetApplications();
            var applicationList =
                applications.Select(f => new SelectListItem() {Text = f.Name, Value = f.Id.ToString()}).ToList();

            ViewData["ApplicationId"] = applicationList;
        }

        [HttpPost]
        public ActionResult NewRequest(Request newRequest, string message, HttpPostedFileBase newFile)
        {
            var r = Repository.FindRequest(1005);
            SendNewRequestMail(r);
            
            newRequest.DateCreated = DateTime.Now;
            newRequest.BrandId = ViewBag.BrandId;
            newRequest.UserId = ViewBag.UserId;
            newRequest.Status = "New";

            RequestMessage reqMessage = new RequestMessage();
            reqMessage.DateCreated = DateTime.Now;
            reqMessage.Message = message;
            reqMessage.UserId = ViewBag.UserId;

            if (newFile != null && newFile.ContentLength > 0)
            {
                byte[] file = new byte[newFile.InputStream.Length];
                newFile.InputStream.Read(file, 0, file.Length);

                reqMessage.AttachmentFilename = newFile.FileName;
                reqMessage.AttachmentData = file;
            }
            
            newRequest.RequestMessages.Add(reqMessage);

            var requestId = Repository.SaveRequest(newRequest);
            
            var tfsId = TfsHelper.CreateTask(newRequest.Title, reqMessage.Message, newRequest.RequestType, null, requestId.ToString(), newRequest.Priority.GetPriority());

            if (!string.IsNullOrEmpty(tfsId))
            {
                newRequest.TfsId = int.Parse(tfsId);
                Repository.UpdateRequest(newRequest);
            }

            SendNewRequestMail(newRequest);
            return View("RequestSaved", requestId);
        }

     

        public ActionResult ViewRequests()
        {
            return View();
        }

        public ActionResult AllRequests(string startDate, string endDate)
        {
            var userBrandId = ViewBag.BrandId;
            var role = ViewBag.Role;
            List<Request> allRequests; 
            
            if(role != "User")
                allRequests = Repository.GetRequestsByDate(DateTime.Parse(startDate), DateTime.Parse(endDate).AddDays(1));
            else
                allRequests = Repository.GetRequestsByDateBrand(DateTime.Parse(startDate), DateTime.Parse(endDate).AddDays(1), userBrandId);

            return PartialView("_ViewRequests", allRequests);
        }

        public ActionResult UpdateRequest(int requestId)
        {
            var request = Repository.FindRequest(requestId);
            return View(request);
        }

        [HttpPost]
        public ActionResult UpdateRequest(Request request, string description, HttpPostedFileBase newFile)
        {
            var thisRequest = Repository.FindRequest(request.Id);
            var requestMessage = new RequestMessage
            {
                DateCreated = DateTime.Now,
                Message = description,
                UserId = ViewBag.UserId
            };

            if (newFile != null && newFile.ContentLength > 0)
            {
                byte[] file = new byte[newFile.InputStream.Length];
                newFile.InputStream.Read(file, 0, file.Length);

                requestMessage.AttachmentFilename = newFile.FileName;
                requestMessage.AttachmentData = file;
            }

            thisRequest.RequestMessages.Add(requestMessage);
            Repository.UpdateRequest(thisRequest);

            SendRequestUpdatedMail(thisRequest);
            return View("RequestSaved", request.Id);
        }

    public ActionResult CloseRequest(int requestId)
        {
            var thisRequest = Repository.FindRequest(requestId);
            thisRequest.Status = "Resolved";
            Repository.UpdateRequest(thisRequest);
            return View("RequestSaved", requestId);
        }

        public void ExportToExcel(string startDate, string endDate)
        {
            var path = Server.MapPath("~/") + @"\Reports\";
            //var path = ConfigurationManager.AppSettings["ZipPath"];
            string fileName = Guid.NewGuid() + ".xlsx";

            var allRequests = Repository.GetRequestsByDate(DateTime.Parse(startDate), DateTime.Parse(endDate));
            var excelRequests = allRequests.GetExcelRequests();

            excelRequests.ToExcel(path + fileName, "Helpdesk", false);

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

        public void DownloadAttachment(int id)
        {
            var requestMessage = Repository.GetRequestMessage(id);

            if (requestMessage.AttachmentFilename.ToLower().Contains(".pdf"))
            {
                Response.AddHeader("content-disposition", "inline;filename=" + requestMessage.AttachmentFilename);
                Response.ContentType = "application/pdf";
            }
            else if (requestMessage.AttachmentFilename.ToLower().Contains(".jpg"))
            {
                Response.AddHeader("content-disposition", "inline;filename=" + requestMessage.AttachmentFilename);
                Response.ContentType = "image/jpeg";
            }
            else if (requestMessage.AttachmentFilename.ToLower().Contains(".png"))
            {
                Response.AddHeader("content-disposition", "inline;filename=" + requestMessage.AttachmentFilename);
                Response.ContentType = "image/png";
            }
            else
            {
                Response.AddHeader("content-disposition", "attachment;filename=" + requestMessage.AttachmentFilename);
                Response.ContentType = "application/octet-stream";
            }
            

            Response.BinaryWrite(requestMessage.AttachmentData);
            Response.Flush();
            Response.End();
        }

        public ActionResult Developers()
        {
            var developers = Repository.GetDevelopers();
            var developerList = developers.Select(f => new SelectListItem()
            {
                Text = f.Name,
                Value = f.Id.ToString()
            }).ToList();

            ViewData["Developers"] = developerList;
            return PartialView("_Developers");
        }

        public ActionResult AssignToDeveloper(int requestId, int developerId)
        {
            var thisRequest = Repository.FindRequest(requestId);
            thisRequest.DeveloperId = developerId;
            Repository.UpdateRequest(thisRequest);

            SendRequestAssignedMail(thisRequest);
            return Content(thisRequest.Developer.Name);
        } 
        
        public ActionResult AssignToAppSpecialist(int requestId)
        {
            var thisRequest = Repository.FindRequest(requestId);
            thisRequest.AppSpecialistId = ((User)Session["ThisUser"]).UserAppSpecialists.First().AppSpecialistId;
            Repository.UpdateRequest(thisRequest);

            SendAcceptedRequestdMail(thisRequest);
            return Content(thisRequest.AppSpecialist.Name);
        }

        private void SendNewRequestMail(Request newRequest)
        {
            var message = string.Format("A new request has been created.<br />Request number: {0}<br />Request Title: {1}<br />Request Description : {2}<br /><p>Click <a href='{3}{0}'>here</a> to view it on the Helpdesk.</p>",
                newRequest.Id, newRequest.Title, newRequest.RequestMessages.First().Message, ConfigurationManager.AppSettings["ViewRequestUrl"]);
            var userEmail = Repository.FindUser(newRequest.UserId).EmailAddress;
            var recipients = new List<string> { userEmail };

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["HelpdeskSupportEmail"]))
                recipients.Add(ConfigurationManager.AppSettings["HelpdeskSupportEmail"]);

            var emailHelper = new EmailHelper();
            emailHelper.SendEmail(recipients, "New Request Created " + newRequest.Id, message);
        }

        private void SendRequestAssignedMail(Request thisRequest)
        {
            var message = string.Format("Hi {3}, <p>Request number {0} has been assigned to you.</p><br />Request Title: {1}<br />Request Description : {4}<br /><p>Click <a href='{2}{0}'>here</a> to view it on the Helpdesk.</p>", 
                thisRequest.Id, thisRequest.Title, ConfigurationManager.AppSettings["ViewRequestUrl"], thisRequest.Developer.Name, thisRequest.RequestMessages.First().Message);

            var recipients = new List<string> { thisRequest.Developer.EmailAddress };
            var emailHelper = new EmailHelper();
            emailHelper.SendEmail(recipients, "Request " + thisRequest.Id + " Assigned To You", message);
        } 
        
        private void SendAcceptedRequestdMail(Request thisRequest)
        {
            var message = string.Format("Hi {3}, <p>You have accepted Request number {0} has been assigned to you.</p><br />Request Title: {1}<br />Request Description : {4}<br /><p>Click <a href='{2}{0}'>here</a> to view it on the Helpdesk.</p>",
                thisRequest.Id, thisRequest.Title, ConfigurationManager.AppSettings["ViewRequestUrl"], thisRequest.Developer.Name, thisRequest.RequestMessages.First().Message);

            var recipients = new List<string> { thisRequest.AppSpecialist.EmailAddress };
            var emailHelper = new EmailHelper();
            emailHelper.SendEmail(recipients, "Accepted Request " + thisRequest.Id, message);
        }

        private void SendRequestUpdatedMail(Request thisRequest)
        {
            var user = Session["ThisUser"] as User;

            var message = string.Format("Hi There,<p>Request {0} has been updated by {3}.</p><br />Request Title: {1}<br /><p>Request Description : <br />{4}</p><p>Click <a href='{2}{0}'>here</a> to view it on the Helpdesk.</p>",
                thisRequest.Id, thisRequest.Title, ConfigurationManager.AppSettings["ViewRequestUrl"], user.Name, thisRequest.RequestMessages.First().Message);

            var recipients = new List<string> { thisRequest.User.EmailAddress, thisRequest.Developer.EmailAddress, thisRequest.AppSpecialist.EmailAddress };

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["HelpdeskSupportEmail"]))
                recipients.Add(ConfigurationManager.AppSettings["HelpdeskSupportEmail"]);

            var emailHelper = new EmailHelper();
            emailHelper.SendEmail(recipients, "Request " + thisRequest.Id + " Updated", message);
        }
    }
}