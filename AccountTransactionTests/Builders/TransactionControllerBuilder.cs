using AccountTransactionApi.Controllers;
using AccountTransactionLogic.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace AccountTransactionTests.Builders
{
    public class TransactionControllerBuilder 
    {
        private IAccountTransactionService _accountTransactionService;
        private bool _defaultAccountTransactionService = true;

        public TransactionControllerBuilder WithAccountTransactionService(IAccountTransactionService accountTransactionService)
        {
            _defaultAccountTransactionService = false;
            _accountTransactionService = accountTransactionService;
            return this;
        }

        public TransactionController Build()
        {
            if (_defaultAccountTransactionService)
            {
                _accountTransactionService = DefaultAccountTransactionService().Object;
            }
        
            var controller = new TransactionController(_accountTransactionService);
            controller.Configuration = new HttpConfiguration();
            controller.Request = new HttpRequestMessage();
            return controller;
        }

        public Mock<IAccountTransactionService> DefaultAccountTransactionService()
        {
            var mock = new Mock<IAccountTransactionService>();
            return mock;
        }
    }
}
