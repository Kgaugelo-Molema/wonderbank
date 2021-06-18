using Transactions.Models;
using Transactions.Services;
using Xunit;

namespace TransactionsTests
{
    public class BankAccountTests
    {
        private TransactionsService _transactionService = new TransactionsService(null);

        [Theory]
        [InlineData(AccountType.Savings, 1000)]
        [InlineData(AccountType.Current, 0)]
        public void GivenMinimumDepositWhenOpeningAccountValidateAmount(AccountType accountType, decimal deposit)
        {
            var accountModel = new AccountModel { AccountType = accountType, Amount = deposit };
            var success = _transactionService.OpenAccount(accountModel);
            Assert.True(success);
        }

        [Theory]
        [InlineData(2000)]
        public void GivenAmountWhenMakingDepositIncreaseAccountBalance(decimal amount)
        {
            var accountModel = new AccountModel { Amount = amount, Balance = 0 };
            _transactionService.Deposit(accountModel);
            Assert.Equal(amount, accountModel.Balance);
        }

        [Theory]
        [InlineData(AccountType.Savings, 1)]
        public void GivenSavingsAccountWhenOverdrawingThenDecline(AccountType accountType, decimal withdrawal)
        {
            var approve = _transactionService.Withdraw(new AccountModel { AccountType = accountType, Amount = withdrawal, Balance = 1001 });
            Assert.True(approve);
        }

        [Theory]
        [InlineData(AccountType.Current, 100000)]
        public void GivenCurrentAccountWhenOverdrawingthenDecline(AccountType accountType, decimal withdrawal)
        {
            var approve = _transactionService.Withdraw(new AccountModel { AccountType = accountType, Amount = withdrawal, Balance = 0 });
            Assert.True(approve);
        }
    }
}
