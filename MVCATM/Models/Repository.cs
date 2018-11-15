using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace MVCATM.Models
{
    public class Repository:IRepository
    {
        private ApplicationDbContext applicationDbContext;

        public Repository()
        {
            this.applicationDbContext = new ApplicationDbContext();
        }

        public Repository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public int GetCountCheckingAccounts()
        {
            int count = this.applicationDbContext.CheckingAccounts.Count();
            return count;
        }
        public List<CheckingAccount>GetCheckingAccounts()
        {
            return this.applicationDbContext.CheckingAccounts.ToList();
        }
        public CheckingAccount GetCheckingAccountById(int id)
        {
            CheckingAccount checkingAccount = this.applicationDbContext.CheckingAccounts.Where(c => c.Id == id).FirstOrDefault<CheckingAccount>();
            return checkingAccount;
        }
        public CheckingAccount GetAccountByUserId(string userId)
        {
            
            CheckingAccount checkingAccount = this.applicationDbContext.CheckingAccounts
                                                  .Where(c => c.User.Id == userId).FirstOrDefault<CheckingAccount>();
            return checkingAccount;                                     
        }
        public CheckingAccount CreateCheckingAccount(CheckingAccount checkingAccount)
        {
            var accountNumber = (123456 + GetCountCheckingAccounts()).ToString().PadLeft(10, '0');
            checkingAccount.AccountNumber = accountNumber;
            checkingAccount = AddCheckingAccount(checkingAccount);
            return checkingAccount;
        }


        public TransactionStatus GetStatusById(int id)
        {
            TransactionStatus transactionStatus = this.applicationDbContext
                                                  .TransactionStatuses
                                                  .Where(s => s.ID == id)
                                                  .FirstOrDefault<TransactionStatus>();
            return transactionStatus;
        }

        public Transaction AddTransaction(Transaction transaction)
        {
            this.applicationDbContext.Transactions.Add(transaction);
            this.applicationDbContext.SaveChanges();
            return transaction;
        }

        public Transaction SaveTransaction(Transaction transaction)
        {
            this.applicationDbContext.Entry(transaction).State = EntityState.Modified;
            this.applicationDbContext.SaveChanges();
            return transaction;
        }

        public CheckingAccount AddCheckingAccount(CheckingAccount checkingAccount)
        {
            this.applicationDbContext.CheckingAccounts.Add(checkingAccount);
            this.applicationDbContext.SaveChanges();
            return checkingAccount;
        }

        public CheckingAccount SaveCheckingAccount(CheckingAccount checkingAccount)
        {
            this.applicationDbContext.Entry(checkingAccount).State = EntityState.Modified;
            this.applicationDbContext.SaveChanges();
            return checkingAccount;

        }

        public TransactionStatus AddTransactionStatus(TransactionStatus transactionStatus)
        {
            this.applicationDbContext.TransactionStatuses.Add(transactionStatus);
            this.applicationDbContext.SaveChanges();
            return transactionStatus;
        }

        public TransactionStatus SaveTransactionStatus(TransactionStatus transactionStatus)
        {
            this.applicationDbContext.Entry(transactionStatus).State = EntityState.Modified;
            this.applicationDbContext.SaveChanges();
            return transactionStatus;
        }

        public List<Transaction> GetTransactions()
        {
            return this.applicationDbContext.Transactions.ToList();
        }

        public List<Transaction> GetTransactionsByCheckingAccount(int checkingAccountId)
        {
            var checkingAccount = this.applicationDbContext.CheckingAccounts.Find(checkingAccountId);
            return this.applicationDbContext.Transactions.Include(x => x.TransactionStatus).ToList<Transaction>();
        }
    }
}