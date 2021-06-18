using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transactions.Models
{
    public class AccountModel
    {
        public Guid Id { get; set; }
        public AccountType AccountType { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
    }
}
