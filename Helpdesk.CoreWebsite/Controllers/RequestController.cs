using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Helpdesk.CoreWebsite.Helpers;
using Helpdesk.DataAccess;
using Helpdesk.DataAccess.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;

namespace Helpdesk.CoreWebsite.Controllers
{
    public class RequestController : Controller
    {
        public HelpdeskRepository Repository = new HelpdeskRepository();
        private IConfiguration _configuration;

        public RequestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            PopulateApplications();
            return View();
        }

        public IActionResult NewRequest()
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
                applications.Select(f => new SelectListItem() { Text = f.Name, Value = f.Id.ToString() }).ToList();

            ViewData["ApplicationId"] = applicationList;
        }

        [HttpPost]
        public IActionResult NewRequest(Request newRequest, string message, IFormFile newFile)
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

            if (newFile != null && newFile.Length > 0)
            {
                byte[] file = new byte[newFile.Length];
                newFile.OpenReadStream().Read(file, 0, file.Length);

                reqMessage.AttachmentFilename = newFile.FileName;
                reqMessage.AttachmentData = file;
            }

            newRequest.RequestMessage.Add(reqMessage);

            var requestId = Repository.SaveRequest(newRequest);

            //var tfsId = TfsHelper.CreateTask(newRequest.Title, reqMessage.Message, newRequest.RequestType, null, requestId.ToString(), newRequest.Priority.GetPriority());

            //if (!string.IsNullOrEmpty(tfsId))
            //{
            //    newRequest.TfsId = int.Parse(tfsId);
            //    Repository.UpdateRequest(newRequest);
            //}

            SendNewRequestMail(newRequest);
            return View("RequestSaved", requestId);
        }



        public IActionResult ViewRequests()
        {
            return View();
        }

        public IActionResult AllRequests(string startDate, string endDate)
        {
            var userBrandId = ViewBag.BrandId;
            var role = ViewBag.Role;
            List<Request> allRequests;

            if (role != "User")
                allRequests = Repository.GetRequestsByDate(DateTime.ParseExact(startDate, "dd/MM/yyyy", null), DateTime.ParseExact(endDate, "dd/MM/yyyy", null).AddDays(1));
            else
                allRequests = Repository.GetRequestsByDateBrand(DateTime.ParseExact(startDate, "dd/MM/yyyy", null), DateTime.ParseExact(endDate, "dd/MM/yyyy", null).AddDays(1), userBrandId);

            return PartialView("_ViewRequests", allRequests);
        }

        public IActionResult UpdateRequest(int requestId)
        {
            var request = Repository.FindRequest(requestId);
            return View(request);
        }

        [HttpPost]
        public IActionResult UpdateRequest(Request request, string description, IFormFile newFile)
        {
            var thisRequest = Repository.FindRequest(request.Id);
            var requestMessage = new RequestMessage
            {
                DateCreated = DateTime.Now,
                Message = description,
                UserId = ViewBag.UserId
            };

            if (newFile != null && newFile.Length > 0)
            {
                byte[] file = new byte[newFile.Length];
                
                newFile.OpenReadStream().Read(file, 0, file.Length);

                requestMessage.AttachmentFilename = newFile.FileName;
                requestMessage.AttachmentData = file;
            }

            thisRequest.RequestMessage.Add(requestMessage);
            Repository.UpdateRequest(thisRequest);

            SendRequestUpdatedMail(thisRequest);
            return View("RequestSaved", request.Id);
        }

        public IActionResult CloseRequest(int requestId)
        {
            var thisRequest = Repository.FindRequest(requestId);
            thisRequest.Status = "Resolved";
            Repository.UpdateRequest(thisRequest);
            return View("RequestSaved", requestId);
        }

        public FileContentResult ExportToExcel(string startDate, string endDate)
        {
            var path = AppContext.BaseDirectory + @"\Reports\";
            //var path = _configuration["ZipPath"];
            string fileName = Guid.NewGuid() + ".xlsx";

            var allRequests = Repository.GetRequestsByDate(DateTime.Parse(startDate), DateTime.Parse(endDate));
            var excelRequests = allRequests.GetExcelRequests();

            excelRequests.ToExcel(path + fileName, "Helpdesk", false);

            if (!string.IsNullOrEmpty(fileName))
            {

                //Response.Headers.Add("content-disposition", "attachment;filename=" + fileName.Replace("xlsx", "zip"));
                //Response.ContentType = "application/zip";*
                //Response.BinaryWrite(zippedFile);
                //Response.Flush();
                //Response.End();
            }
            CompressionHelper compress = new CompressionHelper();

            var zippedFile = compress.ZipFileToByteArray(path + fileName, path + fileName.Replace("xlsx", "zip"));

            return File(zippedFile, "application/zip", fileName.Replace("xlsx", "zip"));
        }

        public FileContentResult DownloadAttachment(int id)
        {
            var requestMessage = Repository.GetRequestMessage(id);

            if (requestMessage.AttachmentFilename.ToLower().Contains(".pdf"))
            {
                return File(requestMessage.AttachmentData, "application/pdf", requestMessage.AttachmentFilename);
            }
            else if (requestMessage.AttachmentFilename.ToLower().Contains(".jpg"))
            {
                return File(requestMessage.AttachmentData, "image/jpeg", requestMessage.AttachmentFilename);
            }
            else if (requestMessage.AttachmentFilename.ToLower().Contains(".png"))
            {
                return File(requestMessage.AttachmentData, "image/png", requestMessage.AttachmentFilename);
            }
            else
            {
                return File(requestMessage.AttachmentData, "application/octet-stream", requestMessage.AttachmentFilename);
            }
            
            //Response.BinaryWrite(requestMessage.AttachmentData);
            //Response.Flush();
            //Response.End();
        }

        public IActionResult Developers()
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

        public IActionResult AssignToDeveloper(int requestId, int developerId)
        {
            var thisRequest = Repository.FindRequest(requestId);
            thisRequest.DeveloperId = developerId;
            Repository.UpdateRequest(thisRequest);

            SendRequestAssignedMail(thisRequest);
            return Content(thisRequest.Developer.Name);
        }

        public IActionResult AssignToAppSpecialist(int requestId)
        {
            var thisRequest = Repository.FindRequest(requestId);
            //thisRequest.AppSpecialistId = ((User)Session["ThisUser"]).UserAppSpecialists.First().AppSpecialistId;
            thisRequest.AppSpecialistId = 1;
            Repository.UpdateRequest(thisRequest);

            SendAcceptedRequestdMail(thisRequest);
            return Content(thisRequest.AppSpecialist.Name);
        }

        private void SendNewRequestMail(Request newRequest)
        {
            var message = string.Format("A new request has been created.<br />Request number: {0}<br />Request Title: {1}<br />Request Description : {2}<br /><p>Click <a href='{3}{0}'>here</a> to view it on the Helpdesk.</p>",
                newRequest.Id, newRequest.Title, newRequest.RequestMessage.First().Message, _configuration["ViewRequestUrl"]);
            var userEmail = Repository.FindUser(newRequest.UserId).EmailAddress;
            var recipients = new List<string> { userEmail };

            if (!string.IsNullOrEmpty(_configuration["HelpdeskSupportEmail"]))
                recipients.Add(_configuration["HelpdeskSupportEmail"]);

            var emailHelper = new EmailHelper();
            emailHelper.SendEmail(recipients, "New Request Created " + newRequest.Id, message);
        }

        private void SendRequestAssignedMail(Request thisRequest)
        {
            var message = string.Format("Hi {3}, <p>Request number {0} has been assigned to you.</p><br />Request Title: {1}<br />Request Description : {4}<br /><p>Click <a href='{2}{0}'>here</a> to view it on the Helpdesk.</p>",
                thisRequest.Id, thisRequest.Title, _configuration["ViewRequestUrl"], thisRequest.Developer.Name, thisRequest.RequestMessage.First().Message);

            var recipients = new List<string> { thisRequest.Developer.EmailAddress };
            var emailHelper = new EmailHelper();
            emailHelper.SendEmail(recipients, "Request " + thisRequest.Id + " Assigned To You", message);
        }

        private void SendAcceptedRequestdMail(Request thisRequest)
        {
            var message = string.Format("Hi {3}, <p>You have accepted Request number {0} has been assigned to you.</p><br />Request Title: {1}<br />Request Description : {4}<br /><p>Click <a href='{2}{0}'>here</a> to view it on the Helpdesk.</p>",
                thisRequest.Id, thisRequest.Title, _configuration["ViewRequestUrl"], thisRequest.Developer.Name, thisRequest.RequestMessage.First().Message);

            var recipients = new List<string> { thisRequest.AppSpecialist.EmailAddress };
            var emailHelper = new EmailHelper();
            emailHelper.SendEmail(recipients, "Accepted Request " + thisRequest.Id, message);
        }

        private void SendRequestUpdatedMail(Request thisRequest)
        {
            //var user = Session["ThisUser"] as User;

            var message = string.Format("Hi There,<p>Request {0} has been updated by {3}.</p><br />Request Title: {1}<br /><p>Request Description : <br />{4}</p><p>Click <a href='{2}{0}'>here</a> to view it on the Helpdesk.</p>",
                thisRequest.Id, thisRequest.Title, _configuration["ViewRequestUrl"], User.Identity.Name, thisRequest.RequestMessage.First().Message);

            var recipients = new List<string> { thisRequest.User.EmailAddress, thisRequest.Developer.EmailAddress, thisRequest.AppSpecialist.EmailAddress };

            if (!string.IsNullOrEmpty(_configuration["HelpdeskSupportEmail"]))
                recipients.Add(_configuration["HelpdeskSupportEmail"]);

            var emailHelper = new EmailHelper();
            emailHelper.SendEmail(recipients, "Request " + thisRequest.Id + " Updated", message);
        }
    }
}