using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Helpdesk.Core.Helpers;
using Helpdesk.Repository;

namespace Helpdesk.Website.Controllers
{
    [IsLoggedIn]
    public class HomeController : Controller
    {
        public HelpdeskRepository Repository = new HelpdeskRepository();

        public ActionResult Index()
        {
            var report = Repository.GetReport("RptRequestType", DateTime.Now.AddYears(-5), DateTime.Now);
            
            return View(report);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var user = Repository.FindUserByCredentials(email, password);
            if (user != null)
            {
                Session["ThisUser"] = user;
                ViewBag.ValidCredentials = true;
                return RedirectToActionPermanent("Index");
            }
            else
            {
                ViewBag.ValidCredentials = false;
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session["ThisUser"] = null;
            return RedirectToAction("Index");
        }

        [Auth]
        public ActionResult Register()
        {
            PopulateDropdowns();
            return View();
        }

        [HttpPost]
        public ActionResult Register(User newUser, short roles, short brands)
        {
            var userRole = new UserRole
            {
                RoleId = roles,
                DateCreated = DateTime.Now
            };

            var userBrand = new UserBrand
            {
                BrandId = brands,
                DateCreated = DateTime.Now
            };

            newUser.UserRoles.Add(userRole);
            newUser.UserBrands.Add(userBrand);
            newUser.DateCreated = DateTime.Now;
            newUser.DateModified = DateTime.Now;
            newUser.Active = true;

            var userId = Repository.SaveUser(newUser);
            return RedirectToAction("Users");
        }

        public ActionResult Users()
        {
            var users = Repository.GetUsers();
            return View(users);
        }

        public ActionResult About()
        {
            return View();
        }

        private void PopulateDropdowns()
        {
            var roles = Repository.GetRoles();
            var roleList = roles.Select(f => new SelectListItem() {Text = f.Name, Value = f.Id.ToString()}).ToList();

            var brands = Repository.GetBrands();
            var brandList = brands.Select(f => new SelectListItem() { Text = f.Name, Value = f.Id.ToString() }).ToList();

            ViewData["Roles"] = roleList;
            ViewData["Brands"] = brandList;
        }

        public ActionResult ManageUser(int id)
        {
            var thisUser = Repository.FindUser(id);
            return View(thisUser);
        }

        [HttpPost]
        public ActionResult ManageUser(User user)
        {
          
            //var userId = Repository.SaveUser(newUser);
            return RedirectToAction("Users");
        }
    }
}