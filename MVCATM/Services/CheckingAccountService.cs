using MVCATM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCATM.Services
{
    
    public class CheckingAccountService
    {
        private Repository repository = new Repository();

        public CheckingAccountService(ApplicationDbContext applicationDbContext)
        {
            this.repository = new Repository(applicationDbContext);
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
    }
}