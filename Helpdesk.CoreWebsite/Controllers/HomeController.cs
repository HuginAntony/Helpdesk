using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Helpdesk.CoreWebsite.Helpers;
using Microsoft.AspNetCore.Mvc;
using Helpdesk.CoreWebsite.Models;
using Helpdesk.DataAccess;

namespace Helpdesk.CoreWebsite.Controllers
{
    public class HomeController : Controller
    {
        private HelpdeskContext _dbContext;
        public HelpdeskRepository Repository = new HelpdeskRepository();

        public HomeController(HelpdeskContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var report = Repository.GetReport("RptRequestType", DateTime.Now.AddYears(-5), DateTime.Now);

            return View(report);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
