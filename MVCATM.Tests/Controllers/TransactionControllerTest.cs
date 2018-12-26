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
    public class TransactionControllerTest
    {
        private  Mock<IRepository> mockRepository;
        private CheckingAccount mockCheckingAccount;
        private List<CheckingAccount> checkingAccounts;
        private Transaction mockValidTransaction;
        private Transaction mockInvalidTransaction;
        private TransactionStatus mockDepositTransactionStatus;
        private List<TransactionStatus> transactionStatuses;
        private List<Transaction> transactions;

        //Transfer
        private CheckingAccount toCheckingAccount;
        private TransferViewModel mockTransfer;
        private TransferViewModel invalidAccountTransfer;
        private TransferViewModel invalidAmountTransfer;

        public TransactionControllerTest()
        {
           this.mockRepository = new Mock<IRepository>();

           this.mockCheckingAccount = new CheckingAccount
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
            this.toCheckingAccount = new CheckingAccount
            {
                AccountNumber = "5555123",
                ApplicationUserId = "toUserId",
                Balance = 0,
                FirstName = "To",
                LastName = "User",
                Id = 1000,
                Transactions = null,
                User = new ApplicationUser { Pin = "5555" }
            };


            this.mockValidTransaction = new Transaction
            {
                Amount = 10,
                checkingAccount = mockCheckingAccount,
                CheckingAccountId = 100
            };
            this.mockInvalidTransaction = new Transaction
            {
                Amount = 1000,
                checkingAccount = mockCheckingAccount,
                CheckingAccountId = 100
            };

           this.mockDepositTransactionStatus = new TransactionStatus
            {
                ID = 1,
                processStatus = TransactionProcessStatus.Success,
                StatusMessage = "Transaction of amount 100 to 9999123 on 18:50 by Test User was success",
                transaction = mockValidTransaction,
                TransactionId = 1,
                TransactionTime = Convert.ToDateTime("2018-11-12 18:50:22.550")
            };
            this.mockTransfer = new TransferViewModel
            {
                Amount = 10,
                FromCheckingAccount = mockCheckingAccount,
                FromCheckingAccountId = mockCheckingAccount.Id,
                Id = 250,
                ToAccountNumber = "5555123",
                ToCheckingAccount = toCheckingAccount,
                ToCheckingAccountId = toCheckingAccount.Id,
            };
            this.invalidAccountTransfer = new TransferViewModel
            {
                Amount = 10,
                FromCheckingAccount = mockCheckingAccount,
                FromCheckingAccountId = mockCheckingAccount.Id,
                Id = 250,
                ToAccountNumber = "5555127",
                ToCheckingAccount = toCheckingAccount,
                ToCheckingAccountId = toCheckingAccount.Id,
            };
            this.invalidAmountTransfer = new TransferViewModel
            {
                Amount = 1000,
                FromCheckingAccount = mockCheckingAccount,
                FromCheckingAccountId = mockCheckingAccount.Id,
                Id = 250,
                ToAccountNumber = "5555123",
                ToCheckingAccount = toCheckingAccount,
                ToCheckingAccountId = toCheckingAccount.Id,
            };
            this.checkingAccounts = new List<CheckingAccount>
           {
            this.mockCheckingAccount,
            this.toCheckingAccount
          };
            this.transactionStatuses = new List<TransactionStatus>
            {
            mockDepositTransactionStatus
           };
            this.transactions = new List<Transaction>
          {
            new Transaction{Amount=100,checkingAccount=this.mockCheckingAccount,CheckingAccountId =100,Id=1,TransactionStatus=mockDepositTransactionStatus,TransactionStatusId=mockDepositTransactionStatus.ID}

          };           

           
            mockRepository.Setup(
               m => m.GetCheckingAccountById(It.IsAny<int>())
                               ).Returns(mockCheckingAccount);

            mockRepository.Setup(
                mr => mr.AddTransaction(It.IsAny<Transaction>())
                ).Returns(
                (Transaction transaction) =>
                {
                    transaction.Id = transactions.Count + 1;
                    transactions.Add(transaction);
                    return transaction;
                }
                );

            mockRepository.Setup(
                mr => mr.GetTransactions()).Returns(transactions);

            mockRepository.Setup
                (mr => mr.SaveCheckingAccount(It.IsAny<CheckingAccount>())
                ).Returns(
                (CheckingAccount checkingAccount) =>
                {
                    var original = checkingAccounts.Where(
                            ch => ch.Id == checkingAccount.Id).Single();
                    original.Balance = checkingAccount.Balance;
                    return checkingAccount;
                }
                );

            mockRepository.Setup(
               mr => mr.AddTransactionStatus(It.IsAny<TransactionStatus>())
               ).Returns(
               (TransactionStatus transactionstatus) =>
               {
                   transactionstatus.ID = transactionStatuses.Count + 1;
                   transactionStatuses.Add(transactionstatus);
                   return transactionstatus;
               }
               );
            mockRepository.Setup(
            mr => mr.GetStatusById(It.IsAny<int>())
            ).Returns(
            (int id) =>
            {
                return transactionStatuses.Where(t => t.ID == id).FirstOrDefault<TransactionStatus>();
            }
            );

            mockRepository.Setup(
                mr => mr.GetAccountByNumber(It.IsAny<string>())
                ).Returns(
              (string accountNumber) => 
              {
                  return checkingAccounts.Where(t => t.AccountNumber == accountNumber).FirstOrDefault<CheckingAccount>();
              }  );
        }

        [TestMethod]
        public void Deposit_Get_View_Deposit_With_Transaction()
        {
            //Arrange

            var controller = new TransactionController(mockRepository.Object);

            

            //Act
            var result = controller.Deposit(100) as ViewResult;
            var transaction = (Transaction)result.ViewData.Model;

            //Assert
            Assert.AreEqual(100, transaction.checkingAccount.Id);
        }

        [TestMethod]
        public void Create_Deposit_Valid_Redirect_TransactionStatus()
        {
            //Arrange
            var controller = new TransactionController(mockRepository.Object);
         

            RedirectToRouteResult result = (RedirectToRouteResult)controller.Deposit(mockValidTransaction);
            //Assert
            Assert.AreEqual("Details", result.RouteValues["Action"]);
            Assert.AreEqual("TransactionStatus", result.RouteValues["Controller"]);

        }
        [TestMethod]
        public void Create_Deposit_Valid_Result_TransactionStatus()
        {
            //Arrange
            var controller = new TransactionController(mockRepository.Object);

            
            //verify pre insert
            int transactionCount = transactions.Count;
            Assert.AreEqual(1, transactionCount); // Verify the expected Number pre-insert

            //Act
            //Deposit of 10
            var result = controller.Deposit(mockValidTransaction) as RedirectToRouteResult;
            int resultStatusId = Convert.ToInt32(result.RouteValues["Id"]);
            //Assert

            Assert.AreEqual(2, resultStatusId);
            Assert.AreEqual(2, transactions.Count);

            Transaction latestTran = transactions
                .Where(t => t.CheckingAccountId == mockValidTransaction.CheckingAccountId).LastOrDefault<Transaction>();

            TransactionStatus latestStatus = transactionStatuses
               .Where(t => t.TransactionId == latestTran.Id).LastOrDefault<TransactionStatus>();
            Assert.IsNotNull(latestStatus); // Test if null

            Assert.AreEqual(TransactionProcessStatus.Success, latestStatus.processStatus); // Verify it has success status

            CheckingAccount mockCheckingAccount = latestTran.checkingAccount;
            Decimal balance = mockCheckingAccount.Balance;
            Assert.AreEqual(110, balance);
        }
        [TestMethod]
        public void Create_Withdrawal_Valid_Result_TransactionStatus()
        {
            //Arrange
            var controller = new TransactionController(mockRepository.Object);
            
            //verify pre insert
            int transactionCount = transactions.Count;
            Assert.AreEqual(1, transactionCount); // Verify the expected Number pre-insert

            //Act
            var result = controller.Withdraw(mockValidTransaction) as RedirectToRouteResult;
            int resultStatusId = Convert.ToInt32(result.RouteValues["Id"]);
            //Assert
            transactionCount = transactions.Count;
            Assert.AreEqual(2, transactionCount);

            Transaction latestTran = transactions
                .Where(t => t.CheckingAccountId == mockValidTransaction.CheckingAccountId).LastOrDefault<Transaction>();
            TransactionStatus latestStatus = transactionStatuses
               .Where(t => t.TransactionId == latestTran.Id).LastOrDefault<TransactionStatus>();
            Assert.IsNotNull(latestStatus);

            CheckingAccount checkingAccount = checkingAccounts.Where(t => t.Id == latestTran.CheckingAccountId).FirstOrDefault<CheckingAccount>();
            Decimal currentBalance = checkingAccount.Balance;
            //Ran a withdrawal of 10
            Assert.AreEqual(currentBalance, 90);
            Assert.AreEqual(TransactionProcessStatus.Success, latestStatus.processStatus); // Verify it has the expected status
        }
        [TestMethod]
        public void Create_Withdrawal_Insufficient_Balance()
        {
            //Arrange
            var controller = new TransactionController(mockRepository.Object);
           

            //Act
            var result = controller.Withdraw(mockInvalidTransaction) as ViewResult;
            bool modelState = result.ViewData.ModelState.IsValid;
            //Assert

            Assert.IsFalse(modelState);

        }
        [TestMethod]
        public void Create_QuickCash_Valid_Redirect_TransactionStatus()
        {
            //Arrange
            var controller = new TransactionController(mockRepository.Object);
         
            //verify pre insert
            int transactionCount = transactions.Count;
            Assert.AreEqual(1, transactionCount); // Verify the expected Number pre-insert

            CheckingAccount checkingAccount = checkingAccounts.Where(t => t.Id == mockCheckingAccount.Id).FirstOrDefault<CheckingAccount>();
            Decimal currentBalance = checkingAccount.Balance;
            if (currentBalance >= 100)
            {
                //Act
                var result = controller.QuickCash(mockCheckingAccount.Id) as RedirectToRouteResult;
                int resultStatusId = Convert.ToInt32(result.RouteValues["Id"]);


                TransactionStatus latestStatus = transactionStatuses
                   .Where(t => t.TransactionId == resultStatusId).FirstOrDefault<TransactionStatus>();
                Assert.IsNotNull(latestStatus);

                checkingAccount = checkingAccounts.Where(t => t.Id == mockCheckingAccount.Id).FirstOrDefault<CheckingAccount>();
                currentBalance = checkingAccount.Balance;
                //Ran a withdrawal of 100
                Assert.AreEqual(currentBalance, 0);
                Assert.AreEqual(TransactionProcessStatus.Success, latestStatus.processStatus); // Verify it has the expected status
            }
            else
            {
                //Act
                var result = controller.QuickCash(mockCheckingAccount.Id) as ViewResult;
                bool modelState = result.ViewData.ModelState.IsValid;
                //Assert

                Assert.IsFalse(modelState);
            }

        }

        [TestMethod]
        public void Transfer_Success_StatusView()
        {
            //Arrange
            var controller = new TransactionController(mockRepository.Object);

            //verify pre insert
            decimal frmAccntBalance = mockCheckingAccount.Balance;
            decimal toAccntBalance = toCheckingAccount.Balance;
            Assert.AreEqual(frmAccntBalance, 100); // Verify the expected Number pre-insert
            Assert.AreEqual(toAccntBalance, 0);

            //Act
            //var result = controller.Transfer(mockTransfer) as RedirectToRouteResult;
            //int resultStatusId = Convert.ToInt32(result.RouteValues["Id"]);
            PartialViewResult partialViewResult = controller.Transfer(mockTransfer) as PartialViewResult;
            //Assert
            Assert.AreEqual("_TransactionStatus", partialViewResult.ViewName);
            TransactionStatus resultStatus = (TransactionStatus)partialViewResult.ViewData.Model;
            
            Assert.AreEqual(TransactionProcessStatus.Success,resultStatus.processStatus);

     
        }

        [TestMethod]
        public void Transfer_Failure_AccountNotExists()
        {
            //Arrange
            var controller = new TransactionController(mockRepository.Object);

            //verify pre insert
            decimal frmAccntBalance = mockCheckingAccount.Balance;
            decimal toAccntBalance = toCheckingAccount.Balance;
            Assert.AreEqual(frmAccntBalance, 100); // Verify the expected Number pre-insert
            Assert.AreEqual(toAccntBalance, 0);

            //Act
            var result = controller.Transfer(invalidAccountTransfer) as PartialViewResult;
             bool modelState = result.ViewData.ModelState.IsValid;
             
              //Assert

            Assert.IsFalse(modelState);
            Assert.IsTrue(result.ViewData.ModelState["ToAccountNumber"].Errors.Count > 0);
            Assert.IsTrue(result.ViewData.ModelState["ToAccountNumber"].Errors[0].ErrorMessage == "Account does not exist");

        }

        [TestMethod]
        public void Transfer_Failure_InsufficientBalance()
        {
            //Arrange
            var controller = new TransactionController(mockRepository.Object);

            //verify pre insert
            decimal frmAccntBalance = mockCheckingAccount.Balance;
            decimal toAccntBalance = toCheckingAccount.Balance;
            Assert.AreEqual(frmAccntBalance, 100); // Verify the expected Number pre-insert
            Assert.AreEqual(toAccntBalance, 0);

            //Act
            var result = controller.Transfer(invalidAmountTransfer) as PartialViewResult;
            bool modelState = result.ViewData.ModelState.IsValid;

            //Assert

            Assert.IsFalse(modelState);
            Assert.IsTrue(result.ViewData.ModelState["Amount"].Errors.Count > 0);
            Assert.IsTrue(result.ViewData.ModelState["Amount"].Errors[0].ErrorMessage == "Insufficient balance.Cannot proceed withdrawal");

        }
    }
}