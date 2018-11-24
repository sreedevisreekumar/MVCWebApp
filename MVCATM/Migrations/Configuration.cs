namespace MVCATM.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using MVCATM.Controllers;
    using MVCATM.Models;
    using MVCATM.Services;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MVCATM.Models.ApplicationDbContext>
    {
        public IRepository repository;
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            repository = new Repository(new ApplicationDbContext());
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(MVCATM.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var checkingAccount = new CheckingAccount();
            if (!context.Users.Any(t => t.UserName == "admin@mvcatm.com"))
            {
                var user = new ApplicationUser { UserName = "admin@mvcatm.com", Email = "admin@mvcatm.com", Pin = "8975" };
                userManager.Create(user, "passW0rd!");

                context.Roles.AddOrUpdate(r => r.Name, new IdentityRole { Name = "Admin" });
                context.SaveChanges();

                userManager.AddToRole(user.Id, "Admin");

                Repository repository = new Repository(context);

                var service = new CheckingAccountService(repository);
                service.CreateCheckingAccount("admin", "user", user.Id, 0);

                checkingAccount = repository.GetAccountByUserId(user.Id);
                
                Transaction transactionInitial = new Transaction { Amount = 1000, checkingAccount = checkingAccount, CheckingAccountId = checkingAccount.Id };
                service.MakeTransaction(transactionInitial);               
                

                var transaction = new Transaction { Amount = 100, CheckingAccountId = checkingAccount.Id };
                List<Transaction> transactions = new List<Transaction> { transaction};
                for(int i=0;i<20;i++)
                {
                    if(i%2 == 0)
                    {
                        transaction = new Transaction { Amount = 200, CheckingAccountId = checkingAccount.Id };
                    }
                    else
                    {
                        transaction = new Transaction { Amount = -50, CheckingAccountId = checkingAccount.Id };
                    }
                    transactions.Add(transaction);
                }
                foreach(Transaction trans in transactions)
                {
                    service.MakeTransaction(trans);
                }

            }
           

        }
    }
}
