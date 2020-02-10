using DataAccess.Entities;
using DataAccess.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessTests.Builders
{
    public class TestData
    {
        public static Mock<DbSet<Account>> Accounts
        {
            get
            {
                var list = new List<Account>
                {
                    new Account
                    {
                        Id = 1,
                        AccountNumber = "AccountNumber1",
                        CustomerId = 1,
                        DateOpened = new DateTime(2000, 1, 1),
                        LastUpdated = new DateTime(2000, 1, 1),
                        Locked = false
                    },
                    new Account
                    {
                        Id = 2,
                        AccountNumber = "AccountNumber2",
                        CustomerId = 2,
                        DateOpened = new DateTime(2000, 1, 1),
                        LastUpdated = new DateTime(2000, 1, 1),
                        Locked = false
                    }
                };
                return MockDbSetHelper.GetMockDbSet(list.AsQueryable());
            }
        }

        public static Mock<DbSet<Transaction>> Transactions
        {
            get
            {
                var list = new List<Transaction>
                {
                    new Transaction
                    {
                        Account = Accounts.Object.First(),
                        AccountId = Accounts.Object.First().Id,
                        Amount = 100,
                        DateAdded = new DateTime(2000,1,1),
                        TransactionDate = new DateTime(2000,1,1),
                        LastUpdated = new DateTime(2000,1,1),
                        TransactionTypeId = (int)TransactionTypeEnum.Deposit
                    }
                };
                return MockDbSetHelper.GetMockDbSet(list.AsQueryable());
            }
        }
    }
}
