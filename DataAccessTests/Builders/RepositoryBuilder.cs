using DataAccess;
using DataAccess.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessTests.Builders
{
    public class RepositoryBuilder
    {
        private IAccountDbContext _accountDbContext;
        private bool _defaultAccountDbContext = true;

        public RepositoryBuilder WithAccountDbContext
            (IAccountDbContext accountDbContext)
        {
            _defaultAccountDbContext = false;
            _accountDbContext = accountDbContext;
            return this;
        }

        public IRepository Build()
        {
            if (_defaultAccountDbContext) _accountDbContext =
                    DefaultAccountDbContext().Object;

            return new Repository(_accountDbContext);
        }

        public Mock<IAccountDbContext> DefaultAccountDbContext()
        {
            var mock = new Mock<IAccountDbContext>();
            mock.Setup(x => x.Accounts)
                .Returns(TestData.Accounts.Object);
            mock.Setup(x => x.Transactions)
                .Returns(TestData.Transactions.Object);
            return mock;
        }
    }
}
