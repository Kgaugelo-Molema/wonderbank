using System.Collections.Generic;
using Transactions.Models;

namespace Transactions.Services
{
    public interface ITransactionsService
    {
        IEnumerable<TransactionsModel> GetAll();
        TransactionsModel Add(TransactionsModel transaction);
        bool Withdraw(AccountModel accountModel);
        bool OpenAccount(AccountModel accountModel);
        decimal GetAccountBalance();
    }
}