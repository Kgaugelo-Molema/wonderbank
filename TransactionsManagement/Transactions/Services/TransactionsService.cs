using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transactions.Helpers;
using Transactions.Models;

namespace Transactions.Services
{
    public class TransactionsService : ITransactionsService
    {
        private SqlDataContext _context;

        public TransactionsService(SqlDataContext context)
        {
            _context = context;
        }

        public IEnumerable<TransactionsModel> GetAllTransactions()
        {
            return _context.Transactions;
        }

        public IEnumerable<AccountModel> GetAllAccounts()
        {
            return _context.Accounts;
        }

        public TransactionsModel Add(TransactionsModel transaction)
        {
            const string message = "Amounts must consist denominations of R5 or R10";
            if (!this.ValidateDenomination(transaction.Amount))
                throw new AppException(message);

            var record = _context.Transactions.FirstOrDefault(x => x.Id == transaction.Id);
            if (transaction.Id != 0 && record != default)
            {
                if (!string.IsNullOrEmpty(transaction.Description))
                    record.Description = transaction.Description;

                record.Amount = transaction.Amount;
                _context.SaveChanges();
                return record;
            }

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return transaction;
        }

        public bool Withdraw(AccountModel accountModel)
        {
            var result = false;
            switch (accountModel.AccountType)
            {
                case AccountType.Savings:
                    result = ValidateMinimumBalance(accountModel);
                    break;
                default:
                    result = ValidateOverdraft(accountModel);
                    break;
            }
            return result;
        }

        private bool ValidateOverdraft(AccountModel accountModel)
        {
            return accountModel.Balance - accountModel.Amount >= -100000;
        }

        private bool ValidateMinimumBalance(AccountModel accountModel)
        {
            return accountModel.Balance - accountModel.Amount >= 1000;
        }

        public decimal GetAccountBalance(Guid accountId)
        {
            var account = _context.Accounts.Find(accountId);
            return account.Balance;
        }

        public bool Deposit(AccountModel accountModel)
        {
            accountModel.Balance += accountModel.Amount;
            return true;
        }

        public bool OpenAccount(AccountModel accountModel)
        {
            var result = true;
            switch (accountModel.AccountType)
            {
                case AccountType.Savings:
                    result = accountModel.Amount >= 1000m;
                    break;
                default:
                    break;
            }
            if (!result)
                throw new AppException("Savings account can only be opened through a minimum deposit of R1,000.00");

            _context.Accounts.Add(accountModel);
            _context.SaveChanges();
            return result;
        }
    }

    public static class TransactionValidator
    {
        public static bool ValidateDenomination(this TransactionsService service, decimal amount)
        {
            var isDivisibleBy5 = amount % 5 == 0;
            var isDivisibleBy10 = amount % 10 == 0;
            var result = isDivisibleBy5 || isDivisibleBy10;
            return result;
        }
    }

}
