using MVCATM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace MVCATM.Services
{
    
    public class CheckingAccountService
    {
        private IRepository repository;

        public CheckingAccountService(IRepository repository)
        {
            this.repository = repository;
        }
        public void CreateCheckingAccount(string firstName,string lastName,string userId,decimal initialBalance)
        {
            CheckingAccount checkingAccount = new CheckingAccount { FirstName = firstName, LastName = lastName, AccountNumber = "", Balance = initialBalance, ApplicationUserId =userId };
            checkingAccount = repository.CreateCheckingAccount(checkingAccount);
        }
        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        public void UpdateBalance(CheckingAccount checkingAccount)
        {
            checkingAccount.Balance = repository.GetTransactions().Where(t => t.CheckingAccountId == checkingAccount.Id).Sum(c => c.Amount);
            repository.SaveCheckingAccount(checkingAccount);
        }
        public TransactionStatus MakeTransaction(Transaction transaction)
        {
            Decimal amount = transaction.Amount;
            var date = DateTime.Now;
            TransactionStatus transactionStatus = new TransactionStatus { TransactionTime = DateTime.Now };
            CheckingAccount checkingAccount = repository.GetCheckingAccountById(transaction.CheckingAccountId);

            try

            {

                transaction = repository.AddTransaction(transaction);
                UpdateBalance(checkingAccount);

                transactionStatus.processStatus = TransactionProcessStatus.Success;
                transactionStatus.StatusMessage = $"Transaction of {amount} to {checkingAccount.AccountNumber} on {date:HH:mm} by {checkingAccount.Name} was { transactionStatus.processStatus }";
                transactionStatus.transaction = transaction;
                transactionStatus.TransactionId = transaction.Id;
                transactionStatus.TransactionTime = DateTime.Now;
                transactionStatus = repository.AddTransactionStatus(transactionStatus);


            }
            catch (DbUpdateException ex)
            {
                transactionStatus.processStatus = TransactionProcessStatus.Error;
                transactionStatus.StatusMessage = $"Exception {ex.Message} during transaction of {amount} on {checkingAccount.AccountNumber} on {date:HH:mm} by {checkingAccount.Name} ";
                transactionStatus.transaction = transaction;
                transactionStatus.TransactionId = transaction.Id;
                transactionStatus.TransactionTime = DateTime.Now;
                transactionStatus = repository.AddTransactionStatus(transactionStatus);

            }
            catch (Exception)
            {

                
                transactionStatus.processStatus = TransactionProcessStatus.Error;
                transactionStatus.StatusMessage = $"Transaction of {amount} to {checkingAccount.AccountNumber} on {date:HH:mm} by {checkingAccount.Name} was { transactionStatus.processStatus }";
                transactionStatus.transaction = transaction;
                transactionStatus.TransactionId = transaction.Id;
                transactionStatus.TransactionTime = DateTime.Now;
                transactionStatus = repository.AddTransactionStatus(transactionStatus);

            }


            return transactionStatus;
        }
        public TransactionStatus MakeWithDrawal(Transaction transaction)
        {
            transaction.Amount = -transaction.Amount;
           TransactionStatus transactionStatus = MakeTransaction(transaction);
            return transactionStatus;
        }
    }
}