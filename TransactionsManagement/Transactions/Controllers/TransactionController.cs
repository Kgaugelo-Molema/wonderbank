using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Transactions.Models;
using Transactions.Services;
using AutoMapper;
using Transactions.Helpers;

namespace Transactions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private ITransactionsService _transactionsService;
        private List<object> _results = new List<object>();

        public TransactionController(ITransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
        }

        /// <summary>
        /// Open Account
        /// </summary>
        [HttpPost("open")]
        public IActionResult OpenAccount([FromQuery] AccountType accountType, [FromQuery] decimal amount)
        {
            try
            {
                var id = Guid.NewGuid();
                var accountModel = new AccountModel { Id = id, AccountType = accountType, Amount = amount, Balance = amount };
                var result = _transactionsService.OpenAccount(accountModel);

                _transactionsService.SaveAccount(accountModel);

                SaveTransaction("Open", accountModel);
                return Ok(accountModel);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// List all accounts
        /// </summary>
        [HttpGet]
        [Route("get/accounts")]
        public IActionResult GetAllAccounts()
        {
            var accounts = _transactionsService.GetAllAccounts();
            return Ok(accounts);
        }

        /// <summary>
        /// Deposit amount
        /// </summary>
        [HttpPost("deposit")]
        public IActionResult DepositAmount([FromQuery] Guid id, [FromQuery] decimal amount)
        {
            try
            {
                var accountModel = _transactionsService.GetAccount(id);
                accountModel.Amount = amount;
                var result = _transactionsService.Deposit(accountModel);

                SaveTransaction("Deposit", accountModel);
                return Ok(accountModel);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Withdraw amount
        /// </summary>
        [HttpPost("withdraw")]
        public IActionResult WithdrawAmount([FromQuery] Guid id, [FromQuery] decimal amount)
        {
            try
            {
                var accountModel = _transactionsService.GetAccount(id);
                accountModel.Amount = amount;
                var result = _transactionsService.Withdraw(accountModel);

                SaveTransaction("Withdrawal", accountModel);
                return Ok(accountModel);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// List all transactions
        /// </summary>
        [HttpGet]
        [Route("get/transactions")]
        public IActionResult GetAllTransactions()
        {
            var transactions = _transactionsService.GetAllTransactions();

            var dtos = new List<TransactionsDto>();
            IMapper mapper;
            foreach (var transaction in transactions)
            {
                // map model to dto
                mapper = this.GetMapper<TransactionsModel, TransactionsDto>();
                var dto = mapper.Map<TransactionsDto>(transaction);
                dtos.Add(dto);
            }
            return Ok(dtos);
        }

        /// <summary>
        /// List account transactions
        /// </summary>
        [HttpGet]
        [Route("get/transactions/{id}")]
        public IActionResult GetAllTransactions([FromRoute] Guid id)
        {
            var transactions = _transactionsService.GetAllTransactions(id);

            var dtos = new List<TransactionsDto>();
            IMapper mapper;
            foreach (var transaction in transactions)
            {
                // map model to dto
                mapper = this.GetMapper<TransactionsModel, TransactionsDto>();
                var dto = mapper.Map<TransactionsDto>(transaction);
                dtos.Add(dto);
            }
            return Ok(dtos);
        }

        private void SaveTransaction(string operation, AccountModel accountModel)
        {
            var transaction = new TransactionsModel { Description = $"{operation}: Balance R{accountModel.Balance}", Amount = accountModel.Amount, AccountId = accountModel.Id };
            _transactionsService.Add(transaction);
        }
    }
}