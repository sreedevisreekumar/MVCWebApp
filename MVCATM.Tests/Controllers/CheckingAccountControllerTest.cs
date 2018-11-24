using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MVCATM.Controllers;
using MVCATM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVCATM.Tests.Controllers
{
    [TestClass]
  public  class CheckingAccountControllerTest
    {
        private static Mock<IRepository> mockRepository;
        public static CheckingAccount mockCheckingAccount = new CheckingAccount
        {
            AccountNumber = "9999123",
            ApplicationUserId = "mockuserId",
            Balance = 100,
            FirstName = "Test",
            LastName = "User",
            Id = 100,
            Transactions = null,
            User = new ApplicationUser { Pin = "9999" }
        };
        public List<CheckingAccount> checkingAccounts = new List<CheckingAccount>
        {
            mockCheckingAccount
        };

        public static Transaction mockValidTransaction = new Transaction
        {
            Amount = 10,
            checkingAccount = mockCheckingAccount,
            CheckingAccountId = 100
        };
        public static TransactionStatus mockDepositTransactionStatus = new TransactionStatus
        {
            ID = 1,
            processStatus = TransactionProcessStatus.Success,
            StatusMessage = "Transaction of amount 100 to 9999123 on 18:50 by Test User was success",
            transaction = mockValidTransaction,
            TransactionId = 1,
            TransactionTime = Convert.ToDateTime("2018-11-12 18:50:22.550")
        };
        public List<TransactionStatus> transactionStatuses = new List<TransactionStatus>
        {
            mockDepositTransactionStatus
        };
        public List<Transaction> transactions = new List<Transaction>
        {
            new Transaction{Amount=100,checkingAccount=mockCheckingAccount,CheckingAccountId =100,Id=1,TransactionStatus =mockDepositTransactionStatus,TransactionStatusId =mockDepositTransactionStatus.ID }

        };
        public CheckingAccountControllerTest()
        {
            mockRepository = new Mock<IRepository>();
            mockRepository.Setup(
               m => m.GetCheckingAccountById(It.IsAny<int>())
                               ).Returns(mockCheckingAccount);

            

            mockRepository.Setup(
                mr => mr.GetTransactionsByCheckingAccount(It.IsAny<int>())).Returns(
                (int accountId)=>
                transactions.FindAll(t=>t.CheckingAccountId ==accountId).ToList<Transaction>());

            
          
        }

        [TestMethod]
        public void Check_Statement_Valid()
        {
            //Arrage
            var controller = new CheckingAccountController(mockRepository.Object);

     
            //Act
            var result = controller.Statement(mockCheckingAccount.Id) as ViewResult;
            List<Transaction> statements = ((ViewResult)result).Model as List<Transaction>;

            bool validId = true;
            Assert.IsNotNull(statements);

            Assert.AreEqual(1, statements.Count);
            foreach(Transaction trans in statements)
            {
                if(trans.CheckingAccountId !=mockCheckingAccount.Id)
                {
                    validId = false;
                }
            }
            Assert.AreEqual(true, validId);

        }
    }
}
