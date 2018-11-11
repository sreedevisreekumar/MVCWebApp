using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MVCATM.Models;

namespace MVCATM.Controllers
{
    public class HomeController : Controller
    {
        private Repository repository = new Repository();
        [Authorize]
        public ActionResult Index()
        {
            string applicationUserId = User.Identity.GetUserId();
            CheckingAccount checkingAccount = repository.GetAccountByUserId(applicationUserId);
            var checkingAccountId = checkingAccount.Id;
            ViewBag.CheckingAccountId = checkingAccountId;
            var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = manager.FindById(applicationUserId);
            ViewBag.Pin = user.Pin;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Having trouble? Send us a message.";

            return View();
        }
        [HttpPost]
        public ActionResult Contact(string message)
        {
            ViewBag.Message = "Thanks!We got your message";

            return View();
        }

        public ActionResult Serial(string letterCase)
        {
            var serial = "ASPNETMVC5ATM1";
            if(letterCase == "lower")
            {
                return Content(serial.ToLower());
            }
            return Content(serial);
        }
    }
}