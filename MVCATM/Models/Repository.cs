using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


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
        public CheckingAccount GetAccountByUserId(string userId)
        {
            
            CheckingAccount checkingAccount = this.applicationDbContext.CheckingAccounts
                                                  .Where(c => c.User.Id == userId).FirstOrDefault<CheckingAccount>();
            return checkingAccount;                                     
        }

    }
}