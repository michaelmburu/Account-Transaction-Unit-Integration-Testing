using System;
using System.Threading.Tasks;
using AccountTransactionLogic.Models;
using DataAccess.Entities;
using DataAccess.Enums;
using DataAccess.Interfaces;
using AccountTransactionLogic.Interfaces;
using AccountTransactionLogic.Helpers;

namespace AccountTransactionLogic
{
    public class AccountTransactionService : IAccountTransactionService
    {
        private readonly IRepository _repository;
        private readonly INotifyAccountSummary _notifyAccountSummary;

        public AccountTransactionService(
            IRepository repository, 
            INotifyAccountSummary notifyAccountSummary)
        {
            _repository = repository;
            _notifyAccountSummary = notifyAccountSummary;
        }

        public async Task Log(string message, LogLevels level)
        {
            await _repository.Log(message, level);
        }

        public int CommitTransaction(TransactionInfo transactionInfo)
        {
            var account = GetAccount(transactionInfo.AccountNumber);
            var transaction = new Transaction
            {
                Account = account,
                AccountId = account.Id,
                Amount = transactionInfo.Amount,
                DateAdded = DateTime.UtcNow,
                TransactionDate = DateTime.UtcNow,
                TransactionTypeId = (int) TransactionTypeHelper.GetType(transactionInfo.Amount)
            };

            return _repository.AddTransaction(transaction);
        }

        private Account GetAccount(string accountNumber)
        {
            var account  = _repository.GetAccount(accountNumber);
            if (account == null)
            {
                throw new ArgumentException($"Account Number {accountNumber} not found");
            }
            return account;
        }

        public void TransactionCleared(int transactionId)
        {
            _repository.UpdateTransaction(transactionId, cleared: true);
        }

        public void TransactionFailed(int transactionId)
        {
            _repository.UpdateTransaction(transactionId, cleared: false);
        }

        public async Task SummarizeTransactions(string accountNumber)
        {
            await _notifyAccountSummary.SummarizeTransactions(accountNumber);
        }

      
        
        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
