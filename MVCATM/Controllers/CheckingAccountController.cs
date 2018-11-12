using MVCATM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.Security;

namespace MVCATM.Controllers
{
    [Authorize]
    public class CheckingAccountController : Controller
    {
        private IRepository repository;
        public CheckingAccountController()
        {
            this.repository = new Repository();
        }
        public CheckingAccountController(IRepository repository)
        {
            this.repository = repository;
        }

        // GET: CheckingAccount
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles ="Admin")]
        public ActionResult List()
        {
            List<CheckingAccount> checkingAccounts = repository.GetCheckingAccounts();
            return View(checkingAccounts);
        }
        // GET: CheckingAccount/Details
        public ActionResult Details()
        {
            string applicationUserId = User.Identity.GetUserId();
            CheckingAccount checkingAccount = this.repository.GetAccountByUserId(applicationUserId);
            return View(checkingAccount);
        }
        [Authorize(Roles = "Admin")]
        // GET: CheckingAccount/Details
        public ActionResult DetailsForAdmin(int checkingAccountId)
        {
         
            CheckingAccount checkingAccount = repository.GetCheckingAccountById(checkingAccountId);
            return View("Details",checkingAccount);
        }

        // GET: CheckingAccount/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CheckingAccount/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CheckingAccount/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CheckingAccount/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CheckingAccount/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CheckingAccount/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
