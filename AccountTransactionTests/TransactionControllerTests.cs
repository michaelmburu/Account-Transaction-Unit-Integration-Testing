using AccountTransactionApi.Controllers;
using AccountTransactionLogic.Interfaces;
using AccountTransactionLogic.Models;
using AccountTransactionTests.Builders;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace AccountTransactionTests
{
    [TestClass]
    [TestCategory("Unit")]
    public class TransactionControllerTests
    {
        //Test scenario to check if we can post a valid account
        // The mock service can get easily duplicated
        [TestMethod]
        public async Task PostTransaction_WithValidAccount_Returns200OK()
        {
            //Arrange
            var accountTransactionService = new Mock<IAccountTransactionService>();

            var controller = new TransactionController(accountTransactionService.Object);
            var transactionInfo = new TransactionInfo
            {
                AccountNumber = "1",
                Amount = 100,
                TransactionDate = new DateTime(2000, 1, 1)
            };

            //Act
            var actionResult = await controller.PostTransaction(transactionInfo);

            //Assert
            actionResult.Should().BeOfType(typeof(OkNegotiatedContentResult<int>));
        }

        //Same test as above but we have bundled the arrange code to a builder class
        [TestMethod]
        public async Task PostTransaction_WithValidAccount_Returns200OKRefactored()
        {
            //Arrange
            var builder = new TransactionControllerBuilder();
            var controller = builder.Build();
            var transactionInfo = GetTestableTransactionInfo();
            //Act
            var actionResult = await controller.PostTransaction(transactionInfo);

            //Assert
            actionResult.Should().BeOfType(typeof(OkNegotiatedContentResult<int>));
        }


        //test scenarion where we can't find a valid account when posting a transaction.
        [TestMethod]
        public async Task PostTransaction_WithInvalidAccount_ReturnedNotFound()
        {
            //Arrange
            var builder = new TransactionControllerBuilder();
            var service = builder.DefaultAccountTransactionService();
            service.Setup(x => x.CommitTransaction(It.IsAny<TransactionInfo>())).Throws<ArgumentException>();

            var controller = builder
                .WithAccountTransactionService(service.Object)
                .Build();
            var transactionInfo = GetTestableTransactionInfo();

            //Act
            var actionResult = await controller.PostTransaction(transactionInfo);
            var content = actionResult as NegotiatedContentResult<string>;
            //Assert
            content.StatusCode.Should().Be(HttpStatusCode.NotFound); 
        }

         //Test scenarion where account get summarised after a transaction is posted.
         [TestMethod]
        public async Task PostTransaction_WithValidAccount_SummarisedTransaction()
        {
            //Arrange
            var builder = new TransactionControllerBuilder();
            var service = builder.DefaultAccountTransactionService();
            var controller = builder.WithAccountTransactionService(service.Object).Build();
            var transactionInfo = GetTestableTransactionInfo();
            //Act
            var actionREsult = await controller.PostTransaction(transactionInfo);
            //Assert
            service.Verify(x => x.SummarizeTransactions("1"), Times.Once, "We need to make sure the account is summarised");
        }

        [TestMethod]
        public async Task PostTransaction_OnError_DisposesService()
        {
            //Arrange
            var builder = new TransactionControllerBuilder();
            var service = builder.DefaultAccountTransactionService();
            service.Setup(x => x.CommitTransaction(It.IsAny<TransactionInfo>()))
                .Throws(new Exception("Test Exception"));
            var controller = builder.WithAccountTransactionService(service.Object).Build();
            //Act
            var actionResult = await controller.PostTransaction(new TransactionInfo());
            //Assert
            service.Verify(x => x.Dispose(), Times.Once, "the dispose method needs to be called once to clean up resources");
        }

        public TransactionInfo GetTestableTransactionInfo()
        {
            var transactionInfo = new TransactionInfo
            {
                AccountNumber = "1",
                Amount = 100,
                TransactionDate = new DateTime(2000, 1, 1)
            };
            return transactionInfo; 
        }
    }
}
