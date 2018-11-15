﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCATM.Models;
using MVCATM.Services;

namespace MVCATM.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private IRepository repository;
        private CheckingAccountService checkingAccountService;

       public TransactionController()
        {
            this.repository = new Repository();
           this.checkingAccountService = new CheckingAccountService(this.repository);
        }
        public TransactionController(IRepository iRepository)
        {
            this.repository = iRepository;
            this.checkingAccountService = new CheckingAccountService(this.repository);
        }
        // GET: Transaction
        public ActionResult Index()
        {
            return View();
        }

        // GET: Transaction/Details/5
        public ActionResult Details()
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
        public ActionResult Deposit(int checkingAccountId)
        {
            CheckingAccount checkingAccount = repository.GetCheckingAccountById(checkingAccountId);      

            Transaction transaction = new Transaction {
                                                        Amount = 0,
                                                        checkingAccount = checkingAccount,
                                                        CheckingAccountId = checkingAccountId,
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
                TransactionStatus transactionStatus=checkingAccountService.MakeTransaction(transaction);

                return RedirectToAction("Details","TransactionStatus",routeValues:new { Id=transactionStatus.ID});
            }
            catch
            {
                return View();
            }
        }

        // GET: Transaction/Withdraw
        public ActionResult Withdraw(int checkingAccountId)
        {
            CheckingAccount checkingAccount = repository.GetCheckingAccountById(checkingAccountId);
            Transaction transaction = new Transaction
            {
                Amount = 0,
                checkingAccount = checkingAccount,
                CheckingAccountId = checkingAccountId
            };
            return View(transaction);
        }

        // POST: Transaction/Create
        [HttpPost]
        public ActionResult Withdraw(Transaction transaction)
        {
            
            try
            {
                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here
                    Decimal amount = transaction.Amount;

                    CheckingAccount checkingAccount = repository.GetCheckingAccountById(transaction.CheckingAccountId);
                    Decimal balance = checkingAccount.Balance;
                    var accountId = checkingAccount.Id;
                    transaction.checkingAccount = checkingAccount;
                    if (balance > amount)
                    {

                        TransactionStatus transactionStatus = this.checkingAccountService.MakeWithDrawal(transaction);
                        return RedirectToAction("Details", "TransactionStatus", routeValues: new { Id = transactionStatus.ID });
                    }
                    else
                    {
                      
                         ModelState.AddModelError("Amount", "Insufficient balance.Cannot proceed withdrawal");
                        return View(transaction);
                    }
                }
               else
                {
                    return View(transaction);
                }
            }
            catch
            {
                return View(transaction);
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
