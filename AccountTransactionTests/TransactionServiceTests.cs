using AccountTransactionLogic.Models;
using AccountTransactionTests.Builders;
using DataAccess.Entities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountTransactionTests
{
    [TestClass]
    [TestCategory("Unit")]
    public class TransactionServiceTests
    {
        [TestMethod]
        public void CommitTransaction_OnSuccess_ReturnsTransactionId()
        {

            //Arrange
            var builder = new TransactionServiceBuilder();
            var transactionInfo = GetTestableTransactionInfo();
            var expectedId = 5;
            var repo = builder.DefaultRepository();
            repo.Setup(x => x.GetAccount(It.IsAny<string>()))
                .Returns(new Account());
            repo.Setup(t => t.AddTransaction(It.IsAny<Transaction>()))
                .Returns(expectedId);
            var service = builder.WithRespository(repo.Object).Build();
            //Act
            var resultId = service.CommitTransaction(transactionInfo);
            //Assert
            resultId.Should().Be(expectedId);
        }

        //Test Scenario where if account is not found it throws an argument exception. 
        [TestMethod]

        public void CommitTransaction_AccountNotFound_ThrowsArgumentException()
        {
            //Arrange
            var builder = new TransactionServiceBuilder();
            var repo = builder.DefaultRepository();
            repo.Setup(g => g.GetAccount(It.IsAny<string>())).Returns((Account)null);
            var service = builder.WithRespository(repo.Object).Build();
            var transaction = new TransactionInfo { AccountNumber = "bad" };

            //Act
            Action action = () => service.CommitTransaction(transaction);
            //Assert
            action.Should().Throw<ArgumentException>();
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
