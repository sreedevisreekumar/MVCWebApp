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
        
       public static CheckingAccount mockCheckingAccount = new CheckingAccount
        {
            AccountNumber = "9999123",
            ApplicationUserId = "mockuserId",
            Balance = 100,
            FirstName = "Test",
            LastName = "User",
            Id=100,
            Transactions=null,
            User=new ApplicationUser { Pin = "9999" }
        };
        public static Transaction mockDepositValidTransaction = new Transaction
        {
            Amount=10,
            checkingAccount=mockCheckingAccount,
            CheckingAccountId=100
        };
        [TestMethod]
        public void Deposit_Get_View_Deposit_With_Transaction()
        {
            //Arrange
            Mock<IRepository> mockRepository = new Mock<IRepository>();

            mockRepository.Setup(
                m => m.GetCheckingAccountById(It.IsAny<int>())
                                ).Returns(mockCheckingAccount);
            var controller = new TransactionController(mockRepository.Object);

            //Act
            var result = controller.Deposit(100) as ViewResult;
            var transaction = (Transaction)result.ViewData.Model;

            //Assert
            Assert.AreEqual(100, transaction.checkingAccount.Id);
        }

        [TestMethod]
        public void Create_Deposit_Valid_Return_TransactionStatus()
        {
            //Arrange
            Mock<IRepository> mockRepository = new Mock<IRepository>();
             
        }
    }
}
