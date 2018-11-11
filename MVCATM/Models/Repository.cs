using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace MVCATM.Models
{
    public class Repository
    {
       private ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        public int GetCountCheckingAccounts()
        {
            int count = this.applicationDbContext.CheckingAccounts.Count();
            return count;
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
            this.applicationDbContext.CheckingAccounts.Add(checkingAccount);
            this.applicationDbContext.SaveChanges();
            return checkingAccount;
        }

        public TransactionStatus MakeDeposit(Transaction transaction)
        {
            Decimal amount = transaction.Amount;
            var date = DateTime.Now;
            TransactionStatus transactionStatus = new TransactionStatus{TransactionTime=DateTime.Now};
            CheckingAccount checkingAccount = GetCheckingAccountById(transaction.CheckingAccountId);

            try
            {
            
                this.applicationDbContext.Transactions.Add(transaction);
                this.applicationDbContext.SaveChanges();

               
                checkingAccount.Balance = checkingAccount.Balance + amount;
                this.applicationDbContext.Entry(checkingAccount).State = EntityState.Modified;
                this.applicationDbContext.SaveChanges();

                transactionStatus.StatusMessage = $"Deposited {amount} to {checkingAccount.AccountNumber} on {date:HH:mm} by {checkingAccount.Name}";
                transactionStatus.processStatus = ProcessStatus.Success;
                transactionStatus.transaction = transaction;
                transactionStatus.TransactionId = transaction.Id;
                transactionStatus.TransactionTime = DateTime.Now;
                this.applicationDbContext.TransactionStatuses.Add(transactionStatus);
                this.applicationDbContext.SaveChanges();

            }
            catch (DbUpdateException ex)
            {
                transactionStatus.StatusMessage = $"Exception {ex.Message} on {checkingAccount.AccountNumber} deposit on {date:HH:mm} by {checkingAccount.Name} ";
                transactionStatus.processStatus = ProcessStatus.Error;
                transactionStatus.transaction = transaction;
                transactionStatus.TransactionId = transaction.Id;
                transactionStatus.TransactionTime = DateTime.Now;
                this.applicationDbContext.TransactionStatuses.Add(transactionStatus);
                this.applicationDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                
                transactionStatus.StatusMessage = $"Failed to deposit  {amount} to {checkingAccount.AccountNumber} on {date:HH:mm} by {checkingAccount.Name} ";
                transactionStatus.processStatus = ProcessStatus.Error;
                transactionStatus.transaction = transaction;
                transactionStatus.TransactionId = transaction.Id;
                transactionStatus.TransactionTime = DateTime.Now;
                this.applicationDbContext.TransactionStatuses.Add(transactionStatus);
                this.applicationDbContext.SaveChanges();
            }


            return transactionStatus;
        }

        public bool CanWithdraw(Transaction transaction)
        {
            Decimal amount = transaction.Amount;
            CheckingAccount checkingAccount = transaction.checkingAccount;
            return (checkingAccount.Balance > amount ? true : false);

        }
        public TransactionStatus MakeWithDrawal(Transaction transaction)
        {
            Decimal amount = transaction.Amount;
            var date = DateTime.Now;
            TransactionStatus transactionStatus = new TransactionStatus();
            CheckingAccount checkingAccount = GetCheckingAccountById(transaction.CheckingAccountId); 
            try
            {
               
                this.applicationDbContext.Transactions.Add(transaction);
                this.applicationDbContext.SaveChanges();

                checkingAccount.Balance = checkingAccount.Balance - amount;
                this.applicationDbContext.Entry(checkingAccount).State = EntityState.Modified;
                this.applicationDbContext.SaveChanges();

                transactionStatus.StatusMessage = $"Withdrawn {amount} from {checkingAccount.AccountNumber} on {date:HH:mm} by {checkingAccount.Name}";
                transactionStatus.processStatus = ProcessStatus.Success;
                transactionStatus.transaction = transaction;
                transactionStatus.TransactionId = transaction.Id;
                transactionStatus.TransactionTime = DateTime.Now;
                this.applicationDbContext.TransactionStatuses.Add(transactionStatus);
                this.applicationDbContext.SaveChanges();

            }
            catch (Exception ex)
            {

                transactionStatus.StatusMessage = $"Failed to withdraw  {amount} from {checkingAccount.AccountNumber} on {date:HH:mm} by {checkingAccount.Name} ";
                transactionStatus.processStatus = ProcessStatus.Error;
                transactionStatus.transaction = transaction;
                transactionStatus.TransactionId = transaction.Id;
                transactionStatus.TransactionTime = DateTime.Now;
                this.applicationDbContext.TransactionStatuses.Add(transactionStatus);
                this.applicationDbContext.SaveChanges();
            }


            return transactionStatus;

        }

        public TransactionStatus GetStatusById(int id)
        {
            TransactionStatus transactionStatus = this.applicationDbContext
                                                  .TransactionStatuses
                                                  .Where(s => s.ID == id)
                                                  .FirstOrDefault<TransactionStatus>();
            return transactionStatus;
        }

    }
}