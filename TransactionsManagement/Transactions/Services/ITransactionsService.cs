using System;
using System.Collections.Generic;
using Transactions.Models;

namespace Transactions.Services
{
    public interface ITransactionsService
    {
        IEnumerable<TransactionsModel> GetAllTransactions();
        TransactionsModel Add(TransactionsModel transaction);
        bool Withdraw(AccountModel accountModel);
        bool OpenAccount(AccountModel accountModel);
        bool Deposit(AccountModel accountModel);
        decimal GetAccountBalance(Guid accountId);
        IEnumerable<AccountModel> GetAllAccounts();
        AccountModel GetAccount(Guid id);
        void SaveAccount(AccountModel accountModel);
        IEnumerable<TransactionsModel> GetAllTransactions(Guid id);
    }
}