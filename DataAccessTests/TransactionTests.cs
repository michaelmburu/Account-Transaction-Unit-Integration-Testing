using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Entities;
using DataAccess.Enums;
using DataAccess.Interfaces;
using DataAccessTests.Builders;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DataAccessTests
{
    [TestClass]
    [TestCategory("Unit")]
    public class TransactionTests
    {
        private Transaction GetTestableTransaction(Mock<IAccountDbContext> context)
        {
            var account = TestData.Accounts.Object.First();
            var list = new List<Account> { account };
            context.Setup(x => x.Accounts)
                .Returns(MockDbSetHelper.GetMockDbSet(list.AsQueryable()).Object);

            var transaction = new Transaction
            {
                AccountId = account.Id,
                Account = account,
                Amount = 100,
                DateAdded = new DateTime(2000, 1, 1),
                TransactionDate = new DateTime(2000, 1, 1),
                TransactionTypeId = (int)TransactionTypeEnum.Deposit

            };

            return transaction;
        }

        //Test scenario when add transaction is successful and account unlocks
        [TestMethod]
      
        public void AddTransaction_OnSuccess_AccountUnlocks()
        {
            //Arrange
            var builder = new RepositoryBuilder();
            var context = builder.DefaultAccountDbContext();
            var transaction = GetTestableTransaction(context);
            var service = builder.WithAccountDbContext(context.Object).Build();
            //Act 
            service.AddTransaction(transaction);
            //Assert
            transaction.Account.Locked.Should().BeFalse();
        }

        //Test Scenario when transaction has an error and account does not unlock.
        [TestMethod]
      
        public void AddTransaction_OnError_AccountUnlocks()
        {
            //Arrange
            var builder = new RepositoryBuilder();
            var context = builder.DefaultAccountDbContext();
            var transaction = GetTestableTransaction(context);
            context.Setup(x => x.SaveChanges()).Throws(new UnitTestException("Unit test exception"));

            var service = builder.WithAccountDbContext(context.Object).Build();

            //Act
            Action action = () => service.AddTransaction(transaction);

            //Assert
            action.Should().Throw<UnitTestException>();
            transaction.Account.Locked.Should().BeFalse();
        }


    }

    
}
