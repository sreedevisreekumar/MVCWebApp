using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MVCATM.Models;

namespace MVCATM.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private Repository repository = new Repository();
        // GET: Transaction
        public ActionResult Index()
        {
            return View();
        }

        // GET: Transaction/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Transaction/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Transaction/Create
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
        // GET: Transaction/Deposit
        public ActionResult Deposit()
        {
            string applicationUserId = User.Identity.GetUserId();
            CheckingAccount checkingAccount = repository.GetAccountByUserId(applicationUserId);
            var accountId = checkingAccount.Id;
            Transaction transaction = new Transaction {
                                                        Amount = 0,
                                                        checkingAccount = checkingAccount,
                                                        CheckingAccountId = accountId,
                                                        };
            return View(transaction);
        }

        // POST: Transaction/Deposit
        [HttpPost]
        public ActionResult Deposit(Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return View(transaction);
            }
            try
            {                
                TransactionStatus transactionStatus=repository.MakeDeposit(transaction);

                return RedirectToAction("Details","TransactionStatus",routeValues:new { Id=transactionStatus.ID});
            }
            catch
            {
                return View();
            }
        }

        // GET: Transaction/Withdraw
        public ActionResult Withdraw()
        {
            string applicationUserId = User.Identity.GetUserId();
            CheckingAccount checkingAccount = repository.GetAccountByUserId(applicationUserId);
            var accountId = checkingAccount.Id;
            Transaction transaction = new Transaction
            {
                Amount = 0,
                checkingAccount = checkingAccount,
                CheckingAccountId = accountId
                          };
            return View(transaction);
        }

        // POST: Transaction/Create
        [HttpPost]
        public ActionResult Withdraw(Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return View(transaction);
            }
            try
            {
                // TODO: Add insert logic here
                Decimal amount = transaction.Amount;
                
                CheckingAccount checkingAccount = repository.GetCheckingAccountById(transaction.CheckingAccountId);
                Decimal balance = checkingAccount.Balance;
                var accountId = checkingAccount.Id;
                
                if (balance > amount)
                {
                   
                   TransactionStatus transactionStatus = repository.MakeWithDrawal(transaction);
                    return RedirectToAction("Details", "TransactionStatus", routeValues: new { Id = transactionStatus.ID });
                }
                else
                {
                    String message = "Insufficient balance.Cannot proceed withdrawal";
                    ModelState.AddModelError("",message);
                    return View(transaction);
                }

                
            }
            catch
            {
                return View();
            }
        }

        // GET: Transaction/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Transaction/Edit/5
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

        // GET: Transaction/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Transaction/Delete/5
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
