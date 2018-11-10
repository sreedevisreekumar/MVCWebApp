using MVCATM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCATM.Controllers
{
    public class TransactionStatusController : Controller
    {
        private Repository repository = new Repository();
        // GET: TransactionStatus
        public ActionResult Index()
        {
            return View();
        }

        // GET: TransactionStatus/Details/5
        public ActionResult Details(int id)
        {
            TransactionStatus transactionStatus = repository.GetStatusById(id);
            return View(transactionStatus);
        }

        // GET: TransactionStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TransactionStatus/Create
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

        // GET: TransactionStatus/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TransactionStatus/Edit/5
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

        // GET: TransactionStatus/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TransactionStatus/Delete/5
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
