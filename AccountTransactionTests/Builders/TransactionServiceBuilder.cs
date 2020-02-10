using AccountTransactionLogic;
using AccountTransactionLogic.Interfaces;
using DataAccess.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountTransactionTests.Builders
{
    public class TransactionServiceBuilder
    {
        private IRepository _repository;
        private INotifyAccountSummary _notifyAccountSummary;

        private bool _defaultRepository = true;
        private bool _defaultNotifyAccountSummary = true;


        public TransactionServiceBuilder WithRespository(IRepository repository )
        {
            _defaultRepository = false;
            _repository = repository;
            return this;
        }  
         
        public TransactionServiceBuilder WithoutNotifyAccountSummary(INotifyAccountSummary notifyAccountSummary)
        {
            _defaultNotifyAccountSummary = false;
            _notifyAccountSummary = notifyAccountSummary;
            return this;
        }

        public Mock<IRepository> DefaultRepository()
        {
            var mock = new Mock<IRepository>();
            return mock; 
        }

        public AccountTransactionService Build()
        {
            if (_defaultRepository)
                _repository = DefaultRepository().Object;
            if (_defaultNotifyAccountSummary)
                _notifyAccountSummary = DefaultNotifyAccountSummary().Object;
            return new AccountTransactionService(_repository, _notifyAccountSummary);
        }

        private Mock<INotifyAccountSummary> DefaultNotifyAccountSummary()
        {
            var mock = new Mock<INotifyAccountSummary>();
            return mock;
        }

       
    }
}
