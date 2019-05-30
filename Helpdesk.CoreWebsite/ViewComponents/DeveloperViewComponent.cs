using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helpdesk.CoreWebsite.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Helpdesk.CoreWebsite.ViewComponents
{
    public class DeveloperViewComponent : ViewComponent
    {
        public HelpdeskRepository Repository = new HelpdeskRepository();

        public IViewComponentResult Invoke()
        {
            var developers = Repository.GetDevelopers();
            var developerList = developers.Select(f => new SelectListItem()
            {
                Text = f.Name,
                Value = f.Id.ToString()
            }).ToList();

            ViewData["DevelopersList"] = developerList;
            return View("Developers");
        }
    }
}
